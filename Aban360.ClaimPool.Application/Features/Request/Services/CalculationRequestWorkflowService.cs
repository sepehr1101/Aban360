using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Services;
public interface ICalculationRequestWorkflowService
{
    Task CalculationRequestConfirmAsync(SetCalculationRequestInputDto inputDto, int userCode, CancellationToken cancellationToken);
    Task ReCalculationRequestAsync(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken);
    Task<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> CalculationRequestDisplayAsync(int trackNumber, CancellationToken cancellationToken);
}

internal sealed class CalculationRequestWorkflowService : AbstractBaseConnection, ICalculationRequestWorkflowService
{
    public CalculationRequestWorkflowService(ITrackingQueryService trackingQueryService, IMoshtrakQueryService moshtrakQueryService, IGhestQueryService ghestQueryService, IConfiguration configuration, IKartQueryService kartQueryService) : base(configuration)
    {
        _trackingQueryService = trackingQueryService;
        _trackingQueryService.NotNull(nameof(trackingQueryService));
        _moshtrakQueryService = moshtrakQueryService;
        _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        _ghestQueryService = ghestQueryService;
        _ghestQueryService.NotNull(nameof(ghestQueryService));
        _kartQueryService = kartQueryService;
        _kartQueryService.NotNull(nameof(kartQueryService));
    }

    private readonly ITrackingQueryService _trackingQueryService;
    private readonly IMoshtrakQueryService _moshtrakQueryService;
    private readonly IGhestQueryService _ghestQueryService;
    static int _calculationConfirmedStatus = (int)TrackingStatusEnum.CalculationConfirmed;
    static int _requestOrigin = 12;
    static int _setAssessmentResult = (int)TrackingStatusEnum.SetAssessmentResult;
    static int _reCalculateRequired = (int)TrackingStatusEnum.ReCalculation;
    static int[] _allowedStatus =
    {
        _reCalculateRequired,
        _setAssessmentResult
    };
    public async Task CalculationRequestConfirmAsync(SetCalculationRequestInputDto inputDto, int userCode, CancellationToken cancellationToken)
    {
        TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.GetValid(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, latestTrackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        IEnumerable<InstallmentRequestDataOutputDto> installments = await _ghestQueryService.Get(latestTrackingInfo.StringTrackNumber, latestTrackingInfo.ZoneId);
        CalculationRequestConfirmValidation(installments, latestTrackingInfo, moshtrakInfo.CustomerNumber);
        TrackingInsertDuplicateDto trackingInsertDuplicateDto = new(latestTrackingInfo.TrackNumber, _calculationConfirmedStatus, inputDto.Description, userCode, _requestOrigin, true, false);
        await CalculationRequestConfirmExecuteSqlCommand(trackingInsertDuplicateDto);
    }

    private TrackingInsertDto CalculationRequestConfirmGetTrackingCreateDto(SetCalculationRequestInputDto inputDto, TrackingOutputDto latestTrackingInfo, int userCode)
    {
        return new TrackingInsertDto()
        {
            TrackNumber = latestTrackingInfo.TrackNumber,
            ZoneId = latestTrackingInfo.ZoneId,
            BillId = latestTrackingInfo.BillId,
            ServiceGroupId = latestTrackingInfo.ServiceGroupId,
            StatusId = _calculationConfirmedStatus,
            InsertByUserId = userCode,
            Description = inputDto.Description,
            NotificationMobile = latestTrackingInfo.NotificationMobile,
            NeighbourBillId = latestTrackingInfo.NeighbourBillId,
            RequestOrigin = _requestOrigin,
        };
    }

    private async Task CalculationRequestConfirmExecuteSqlCommand(TrackingInsertDuplicateDto trackingInsertDuplicateDto)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                TrackingCommandService trackingCommandService = new(connection, transaction);
                await trackingCommandService.UpdateIsConsiderdLatest(trackingInsertDuplicateDto.TrackNumber, true);
                await trackingCommandService.InsertDuplicate(trackingInsertDuplicateDto);
                transaction.Commit();
            }
        }
    }

    private void CalculationRequestConfirmValidation(IEnumerable<InstallmentRequestDataOutputDto> installments, TrackingOutputDto latestTrackingInfo, int customerNumber)
    {
        if (!installments.Any())
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidCalculationConfirmed);
        }

        if (customerNumber == 0 || string.IsNullOrWhiteSpace(latestTrackingInfo.BillId))
        {
            throw new InvalidTrackingException(ExceptionLiterals.SetBillId);
        }

        if (!_allowedStatus.Contains(latestTrackingInfo.StatusId))
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
        }
    }

    static int _recalculateStatus = (int)TrackingStatusEnum.ReCalculation;
    static int _deletedSatatus = (int)TrackingStatusEnum.Deleted;
    static int[] _disallowedStatusIds =
    {
        _recalculateStatus,
        _deletedSatatus
    };
    public async Task ReCalculationRequestAsync(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
    {
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
        MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
        ReCalculationRequestValidate(inputDto.TrackNumber, trackingInfo.StatusId, moshtrakInfo.IsRegistered);
        TrackingInsertDuplicateDto trackingInsertDto = ReCalculationRequestGetTrackingInsertDto(inputDto, userCode);
        string dbName = GetDbName(trackingInfo.ZoneId);
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
            {
                TrackingCommandService trackingCommandService = new(connection, transaction);
                await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                await trackingCommandService.InsertDuplicate(trackingInsertDto);
                transaction.Commit();
            }
        }
    }

    private void ReCalculationRequestValidate(int trackNumber, int statusId, bool isRegistered)
    {
        if (isRegistered == true || _disallowedStatusIds.Contains(statusId)) //and not Deleted
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
        }
    }

    private TrackingInsertDuplicateDto ReCalculationRequestGetTrackingInsertDto(TrackNumberWithDescriptionInputDto inputDto, int userCode)
    {
        return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _recalculateStatus, inputDto.Description, userCode, _requestOrigin, true, false);
    }

    private readonly IKartQueryService _kartQueryService;
    static string _title = "مبالغ و تخفیفات";
    public async Task<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> CalculationRequestDisplayAsync(int trackNumber, CancellationToken cancellationToken)
    {
        TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
        IEnumerable<CalculationRequestDisplayDataOutputDto> data = await _kartQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);
        IEnumerable<SaleAndAfterSaleDataOutputDto> resultData = data.Select(s => new SaleAndAfterSaleDataOutputDto(s.Id, s.Title, s.Amount + s.Discount, s.Discount, s.Amount, s.DiscountTypeId, s.DiscountTypeTitle, s.Removable));
        long amount = resultData?.Sum(c => c.Amount) ?? 0;
        long discount = resultData?.Sum(c => c.Discount) ?? 0;
        SaleAndAfterSaleHeaderOutputDto header = new(amount, discount, amount - discount, resultData?.Count() ?? 0); //todo: true or not
        return new ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>(_title, header, resultData);
    }
}
