using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using global::Aban360.ClaimPool.Domain.Constants;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using global::Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using global::Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using global::Aban360.Common.BaseEntities;
using global::Aban360.Common.Db.Dapper;
using global::Aban360.Common.Db.Services;
using global::Aban360.Common.Exceptions;
using global::Aban360.Common.Extensions;
using global::Aban360.Common.Literals;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlBeforeWorkflow10.Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations{
    internal sealed class SetMotherChildRequestHandler : global::WorkflowRecordingConnectionBase, ISetMotherChildRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMotherQueryService _motherQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IValidator<MotherChildRequestInputDto> _validator;
        public SetMotherChildRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IMotherQueryService motherQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IValidator<MotherChildRequestInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _motherQueryService = motherQueryService;
            _motherQueryService.NotNull(nameof(motherQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(MotherChildRequestInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber motherCustomerNumberInfo = await _commonMemberQueryService.Get(inputDto.MotherBillId, true);
            MemberInfoGetDto motherInfo = await _commonMemberQueryService.Get(motherCustomerNumberInfo);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MotherChildValidate(motherInfo, trackingInfo, inputDto);

            MoshtrakOutputDto moshtrackInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            MotherInsertDto motherInsertDto = GetMotherInsertDto(inputDto, moshtrackInfo, motherCustomerNumberInfo.CustomerNumber);

            string dbName = GetDbName(trackingInfo.ZoneId);
            await DuplicateValidation(moshtrackInfo.IsRegistered, motherInsertDto.StringTrackNumber, moshtrackInfo.ZoneId);

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
        private void MotherChildValidate(MemberInfoGetDto motherInfo, TrackingOutputDto trackingInfo, MotherChildRequestInputDto inputDto)
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
        private async Task InputValidate(MotherChildRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task DuplicateValidation(bool isRegistered, string stringTrackNumber, int zoneId)
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
        private MotherInsertDto GetMotherInsertDto(MotherChildRequestInputDto inputDto, MoshtrakOutputDto moshtrackInfo, int motherCustomerNumber)
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
    }
}
