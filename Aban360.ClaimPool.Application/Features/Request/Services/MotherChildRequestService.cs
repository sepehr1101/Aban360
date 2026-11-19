using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;

using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Services;
public interface IMotherChildRequestService
{
    Task UpdateAsync(MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken);
    Task UpdateAsync(CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken);
    Task DeleteAsync(int trackNumber, CancellationToken cancellationToken);
    Task SetMotherChildAsync(MotherChildRequestInputDto inputDto, int userName, CancellationToken cancellationToken);
    Task<MotherChildOutputDto> GetByTrackNumberAsync(int trackNumber, CancellationToken cancellationToken);
    Task SetCommonSiphonAsync(CommonSiphonRequestInputDto inputDto, int userName, CancellationToken cancellationToken);
}

internal sealed class MotherChildRequestService : AbstractBaseConnection, IMotherChildRequestService
{
    private readonly IMotherQueryService _motherQueryService;
    private readonly IMoshtrakQueryService _moshtrakQueryService;
    private readonly ITrackingQueryService _trackingQueryService;
    private readonly IValidator<MotherChildUpdateInputDto> _childValidator;
    private readonly IValidator<CommonSiphonUpdateInputDto> _commonSiphonValidator;
    private readonly ICommonMemberQueryService _commonMemberQueryService;
    private readonly IValidator<MotherChildRequestInputDto> _validator;
    private readonly IValidator<CommonSiphonRequestInputDto> _setCommonSiphonValidator;
    public MotherChildRequestService(IMotherQueryService motherQueryService, IMoshtrakQueryService moshtrakQueryService, ITrackingQueryService trackingQueryService, IValidator<MotherChildUpdateInputDto> childValidator, IValidator<CommonSiphonUpdateInputDto> commonSiphonValidator, IConfiguration configuration, ICommonMemberQueryService commonMemberQueryService, IValidator<MotherChildRequestInputDto> validator, IValidator<CommonSiphonRequestInputDto> setCommonSiphonValidator) : base(configuration)
    {
        _motherQueryService = motherQueryService;
        _motherQueryService.NotNull(nameof(motherQueryService));
        _moshtrakQueryService = moshtrakQueryService;
        _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
        _childValidator = childValidator;
        _childValidator.NotNull(nameof(childValidator));
        _commonSiphonValidator = commonSiphonValidator;
        _commonSiphonValidator.NotNull(nameof(commonSiphonValidator));
        _commonMemberQueryService = commonMemberQueryService;
        _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _setCommonSiphonValidator = setCommonSiphonValidator;
        _setCommonSiphonValidator.NotNull(nameof(setCommonSiphonValidator));
    }

    public async Task UpdateAsync(MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        await UpdateInputValidation(inputDto, cancellationToken);
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        await UpdateMoshtrakValdiation(trackingInfo.ZoneId, inputDto.TrackNumber);
        string dbName = GetDbName(trackingInfo.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MotherCommandService motherCommandService = new(connection, transaction);
                await motherCommandService.Update(inputDto, dbName);
                transaction.Commit();
            }
        }
    }

    public async Task UpdateAsync(CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        await UpdateInputValidation(inputDto, cancellationToken);
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        await UpdateMoshtrakValdiation(trackingInfo.ZoneId, inputDto.TrackNumber);
        string dbName = GetDbName(trackingInfo.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MotherCommandService motherCommandService = new(connection, transaction);
                await motherCommandService.Update(inputDto, dbName);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateMoshtrakValdiation(int zoneId, int trackNumber)
    {
        MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(zoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        if (moshtrakInfo.IsRegistered)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
        }
    }

    private async Task UpdateInputValidation(MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _childValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private async Task UpdateInputValidation(CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _commonSiphonValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task DeleteAsync(int trackNumber, CancellationToken cancellationToken)
    {
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
        string dbName = GetDbName(trackingInfo.ZoneId);
        await DeleteMoshtrakValdiation(trackingInfo.ZoneId, trackNumber);
        string stringTrackNumber = trackNumber.ToString().PadLeft(11, '0');
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MotherCommandService motherCommandService = new(connection, transaction);
                await motherCommandService.Delete(stringTrackNumber, dbName);
                transaction.Commit();
            }
        }
    }

    private async Task DeleteMoshtrakValdiation(int zoneId, int trackNumber)
    {
        MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(zoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        if (moshtrakInfo.IsRegistered)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
        }
    }

    public async Task SetMotherChildAsync(MotherChildRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
    {
        await SetMotherChildInputValidate(inputDto, cancellationToken);
        ZoneIdAndCustomerNumber motherCustomerNumberInfo = await _commonMemberQueryService.Get(inputDto.MotherBillId, true);
        MemberInfoGetDto motherInfo = await _commonMemberQueryService.Get(motherCustomerNumberInfo);
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        SetMotherChildMotherChildValidate(motherInfo, trackingInfo, inputDto);
        MoshtrakOutputDto moshtrackInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        MotherInsertDto motherInsertDto = SetMotherChildGetMotherInsertDto(inputDto, moshtrackInfo, motherCustomerNumberInfo.CustomerNumber);
        string dbName = GetDbName(trackingInfo.ZoneId);
        await SetMotherChildDuplicateValidation(moshtrackInfo.IsRegistered, motherInsertDto.StringTrackNumber, moshtrackInfo.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MotherCommandService motherCommandService = new(connection, transaction);
                await motherCommandService.Insert(motherInsertDto, dbName);
                transaction.Commit();
            }
        }
    }

    private void SetMotherChildMotherChildValidate(MemberInfoGetDto motherInfo, TrackingOutputDto trackingInfo, MotherChildRequestInputDto inputDto)
    {
        if (motherInfo.ZoneId != trackingInfo.ZoneId)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherZoneId);
        }

        if (inputDto.CommercialUnit > motherInfo.CommercialUnit)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidCommercialUnitChild);
        }

        if (inputDto.DomesticUnit > motherInfo.DomesticUnit)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidDomesticUnitChild);
        }

        if (inputDto.OtherUnit > motherInfo.OtherUnit)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidOtherUnitChild);
        }

        if (inputDto.ImprovementCommercial > motherInfo.CommercialImprovement)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidCommercialImprovementChild);
        }

        if (inputDto.ImprovementDomestic > motherInfo.DomesticImprovement)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidDomesticImprovementChild);
        }

        if (inputDto.ImprovementOverall >= motherInfo.OverallImprovement)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidOverallImprovementChild);
        }

        if (inputDto.Premises > motherInfo.Premises)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidPremisesChild);
        }
    }

    private async Task SetMotherChildInputValidate(MotherChildRequestInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private async Task SetMotherChildDuplicateValidation(bool isRegistered, string stringTrackNumber, int zoneId)
    {
        MotherInfoOutputDto? motherInfo = await _motherQueryService.Get(stringTrackNumber, zoneId);
        if (isRegistered)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
        }

        if (!isRegistered && motherInfo is not null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.UseUpdateApi);
        }
    }

    private MotherInsertDto SetMotherChildGetMotherInsertDto(MotherChildRequestInputDto inputDto, MoshtrakOutputDto moshtrackInfo, int motherCustomerNumber)
    {
        return new MotherInsertDto()
        {
            ZoneId = moshtrackInfo.ZoneId,
            CustomerNumber = moshtrackInfo.CustomerNumber,
            ReadingNumber = moshtrackInfo.ReadingNumber ?? string.Empty,
            StringTrackNumber = inputDto.TrackNumber.ToString().PadLeft(11, '0'),
            MotherCustomerNumber = motherCustomerNumber,
            FirstName = moshtrackInfo.FirstName ?? string.Empty,
            Surname = moshtrackInfo.Surname ?? string.Empty,
            FatherName = moshtrackInfo.FatherName,
            Address = moshtrackInfo.Address,
            Siphon100 = 0,
            Siphon125 = 0,
            Siphon150 = 0,
            Siphon200 = 0,
            CommonSiphon = 0,
            MeterDiameterId = moshtrackInfo.MeterDiameterId,
            UsageId = moshtrackInfo.UsageId,
            CommercialUnit = inputDto.CommercialUnit,
            DomesticUnit = inputDto.DomesticUnit,
            OtherUnit = inputDto.OtherUnit,
            Premises = inputDto.Premises,
            ImprovementOverall = inputDto.ImprovementOverall,
            ImprovementCommercial = inputDto.ImprovementCommercial,
            ImprovementDomestic = inputDto.ImprovementDomestic,
            IsSpecial = moshtrackInfo.IsSpecial,
            ContractualCapacity = inputDto.ContractualCapacity,
        };
    }

    public async Task<MotherChildOutputDto> GetByTrackNumberAsync(int trackNumber, CancellationToken cancellationToken)
    {
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
        MoshtrakOutputDto inheritedMoshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        MotherInfoOutputDto? inheritedInfo = await _motherQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);
        if (inheritedInfo is null)
        {
            return new MotherChildOutputDto(null, null, false);
        }

        MemberInfoGetDto? motherInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(trackingInfo.ZoneId, inheritedInfo.MotherCustomerNumber), false);
        if (motherInfo is null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherDataRequest);
        }

        MotherOutputDto inheritedOutput = new()
        {
            TrackNumber = trackNumber,
            StringTrackNumber = trackingInfo.StringTrackNumber,
            BillId = trackingInfo.BillId,
            CustomerNumbe = inheritedMoshtrakInfo.CustomerNumber,
            ZoneId = inheritedInfo.ZoneId,
            MeterDiameterId = inheritedInfo.MeterDiameterId,
            UsageId = inheritedInfo.UsageId,
            IsSpecial = inheritedInfo.IsSpecial,
            Siphon100 = inheritedInfo.Siphon100,
            Siphon125 = inheritedInfo.Siphon125,
            Siphon150 = inheritedInfo.Siphon150,
            Siphon200 = inheritedInfo.Siphon200,
            CommonSiphon = 0, //inheritedInfo.CommonSiphon, //todo: string*
            CommercialUnit = inheritedInfo.CommercialUnit,
            DomesticUnit = inheritedInfo.DomesticUnit,
            OtherUnit = inheritedInfo.OtherUnit,
            Premises = inheritedInfo.Premises,
            ImprovementCommercial = inheritedInfo.ImprovementCommercial,
            ImprovementDomestic = inheritedInfo.ImprovementDomestic,
            ImprovementOverall = inheritedInfo.ImprovementOverall,
            ContractualCapacity = inheritedInfo.ContractualCapacity,
        };
        MotherOutputDto motherOutput = new()
        {
            TrackNumber = 0,
            BillId = motherInfo.BillId,
            StringTrackNumber = string.Empty,
            CustomerNumbe = motherInfo.CustomerNumber,
            ZoneId = motherInfo.ZoneId,
            MeterDiameterId = motherInfo.MeterDiameterId,
            UsageId = motherInfo.UsageId,
            IsSpecial = motherInfo.IsSpecial,
            Siphon100 = motherInfo.Siphon100,
            Siphon125 = motherInfo.Siphon125,
            Siphon150 = motherInfo.Siphon150,
            Siphon200 = motherInfo.Siphon200,
            CommonSiphon = motherInfo.CommonSiphon1,
            CommercialUnit = motherInfo.CommercialUnit,
            DomesticUnit = motherInfo.DomesticUnit,
            OtherUnit = motherInfo.OtherUnit,
            Premises = motherInfo.Premises,
            ImprovementCommercial = motherInfo.CommercialImprovement,
            ImprovementDomestic = motherInfo.DomesticImprovement,
            ImprovementOverall = motherInfo.DomesticImprovement,
            ContractualCapacity = motherInfo.ContractualCapacity,
        };
        return new MotherChildOutputDto(motherOutput, inheritedOutput, true);
    }

    public async Task SetCommonSiphonAsync(CommonSiphonRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
    {
        await SetCommonSiphonInputValidation(inputDto, cancellationToken);
        ZoneIdAndCustomerNumber motherCustomerNumberInfo = await _commonMemberQueryService.Get(inputDto.MotherBillId, true);
        MemberInfoGetDto motherInfo = await _commonMemberQueryService.Get(motherCustomerNumberInfo);
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        SetCommonSiphonCommonSiphonValidate(motherInfo, trackingInfo, inputDto);
        MoshtrakOutputDto moshtrackInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        string dbName = GetDbName(trackingInfo.ZoneId);
        MotherInsertDto motherInsertDto = SetCommonSiphonGetMotherInsertDto(inputDto, moshtrackInfo, motherCustomerNumberInfo.CustomerNumber);
        MoshtrakCommonSiphonUpdateDto moshtrakUpdateDto = new(inputDto.TrackNumber, true);
        await SetCommonSiphonDuplicateValidation(moshtrackInfo.IsRegistered, motherInsertDto.StringTrackNumber, moshtrackInfo.ZoneId);
        await SetCommonSiphonExecSql(motherInsertDto, moshtrakUpdateDto, dbName);
    }

    private async Task SetCommonSiphonExecSql(MotherInsertDto motherInsertDto, MoshtrakCommonSiphonUpdateDto moshtrakUpdateDto, string dbName)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MotherCommandService motherCommandService = new(connection, transaction);
                MoshtrakCommandService moshtrakCommand = new(connection, transaction);
                await motherCommandService.Insert(motherInsertDto, dbName);
                await moshtrakCommand.Update(moshtrakUpdateDto, dbName);
                transaction.Commit();
            }
        }
    }

    private void SetCommonSiphonCommonSiphonValidate(MemberInfoGetDto motherInfo, TrackingOutputDto trackingInfo, CommonSiphonRequestInputDto inputDto)
    {
        if (trackingInfo.ZoneId != motherInfo.ZoneId)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherZoneId);
        }

        if (inputDto.Siphon100 > motherInfo.Siphon100)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon100);
        }

        if (inputDto.Siphon125 > motherInfo.Siphon125)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon125);
        }

        if (inputDto.Siphon150 > motherInfo.Siphon150)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon150);
        }

        if (inputDto.Siphon200 > motherInfo.Siphon200)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidSiphon200);
        }
    }

    private async Task SetCommonSiphonInputValidation(CommonSiphonRequestInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _setCommonSiphonValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private async Task SetCommonSiphonDuplicateValidation(bool isRegistered, string stringTrackNumber, int zoneId)
    {
        MotherInfoOutputDto? motherInfo = await _motherQueryService.Get(stringTrackNumber, zoneId);
        if (isRegistered)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
        }

        if (!isRegistered && motherInfo is not null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.UseUpdateApi);
        }
    }

    private MotherInsertDto SetCommonSiphonGetMotherInsertDto(CommonSiphonRequestInputDto inputDto, MoshtrakOutputDto moshtrackInfo, int motherCustomerNumber)
    {
        return new MotherInsertDto()
        {
            ZoneId = moshtrackInfo.ZoneId,
            CustomerNumber = moshtrackInfo.CustomerNumber,
            ReadingNumber = moshtrackInfo.ReadingNumber ?? string.Empty,
            StringTrackNumber = inputDto.TrackNumber.ToString().PadLeft(11, '0'),
            MotherCustomerNumber = motherCustomerNumber,
            FirstName = moshtrackInfo.FirstName ?? string.Empty,
            Surname = moshtrackInfo.Surname ?? string.Empty,
            FatherName = moshtrackInfo.FatherName,
            Address = moshtrackInfo.Address,
            Siphon100 = inputDto.Siphon100,
            Siphon125 = inputDto.Siphon150,
            Siphon150 = inputDto.Siphon150,
            Siphon200 = inputDto.Siphon200,
            CommonSiphon = moshtrackInfo.CommonSiphon,
            MeterDiameterId = moshtrackInfo.MeterDiameterId,
            UsageId = moshtrackInfo.UsageId,
            CommercialUnit = 0,
            DomesticUnit = 0,
            OtherUnit = 0,
            Premises = 0,
            ImprovementOverall = 0,
            ImprovementCommercial = 0,
            ImprovementDomestic = 0,
            IsSpecial = moshtrackInfo.IsSpecial,
            ContractualCapacity = 0,
        };
    }
}
