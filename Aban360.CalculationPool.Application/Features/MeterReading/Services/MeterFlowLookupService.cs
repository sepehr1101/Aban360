using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services;
public interface IMeterFlowLookupService
{
    Task<MeterFlowGetDto> MeterFlowGetAsync(int id, CancellationToken cancellationToken);
    Task<ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>> MeterReadingDetailLatestInfoByReadingNumberGetAsync(string billIdOrReadingNumber, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class MeterFlowLookupService : IMeterFlowLookupService
{
    private readonly IMeterFlowQueryService _meterFlowService;
    private readonly IBillTransactionDetailsGetHandler _billTransactionDetailsGetHandler;
    private readonly ICommonMemberQueryService _commonMemberQueryService;
    private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
    private readonly IMeterFlowQueryService _meterFlowQueryService;
    private readonly ICommonZoneService _commonZoneService;
    private readonly IMeterReadingCheckedListService _meterReadingCheckedListService;
    public MeterFlowLookupService(IMeterFlowQueryService meterFlowService, IBillTransactionDetailsGetHandler billTransactionDetailsGetHandler, ICommonMemberQueryService commonMemberQueryService, IMeterReadingDetailQueryService meterReadingDetailQueryService, ICommonZoneService commonZoneService, IMeterReadingCheckedListService meterReadingCheckedListService)
    {
        _meterFlowService = meterFlowService;
        _meterFlowService.NotNull(nameof(meterFlowService));
        _billTransactionDetailsGetHandler = billTransactionDetailsGetHandler;
        _billTransactionDetailsGetHandler.NotNull(nameof(billTransactionDetailsGetHandler));
        _commonMemberQueryService = commonMemberQueryService;
        _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _meterFlowQueryService = meterFlowService;
        _meterFlowQueryService.NotNull(nameof(meterFlowService));
        _meterReadingDetailQueryService = meterReadingDetailQueryService;
        _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));
        _meterReadingCheckedListService = meterReadingCheckedListService;
        _meterReadingCheckedListService.NotNull(nameof(meterReadingCheckedListService));
    }

    public async Task<MeterFlowGetDto> MeterFlowGetAsync(int id, CancellationToken cancellationToken)
    {
        MeterFlowGetDto meterFlow = await _meterFlowService.Get(id);
        return meterFlow;
    }

    public async Task<ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>> MeterReadingDetailLatestInfoByReadingNumberGetAsync(string billIdOrReadingNumber, IAppUser appUser, CancellationToken cancellationToken)
    {
        ZoneIdAndCustomerNumberAndBillId customersInfo = await MeterReadingDetailLatestInfoByReadingNumberGetGetCustomerInfo(billIdOrReadingNumber, appUser);
        ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto> transactionInfo = await _billTransactionDetailsGetHandler.Handle(customersInfo.BillId, appUser, cancellationToken);
        var(meterReadingDetail, meterFlowStepEnum) = await MeterReadingDetailLatestInfoByReadingNumberGetGetMeterReadingData(transactionInfo.ReportHeader.BillId);
        BillTransactionDetailWithLastReadingDataHeaderOutputDto header = MeterReadingDetailLatestInfoByReadingNumberGetGetHeader(transactionInfo.ReportHeader, meterReadingDetail, meterFlowStepEnum);
        return new ReportOutput<BillTransactionDetailWithLastReadingDataHeaderOutputDto, BillTransactionDetailDataOutputDto>(transactionInfo.Title, header, transactionInfo.ReportData);
    }

    private async Task<ZoneIdAndCustomerNumberAndBillId> MeterReadingDetailLatestInfoByReadingNumberGetGetCustomerInfo(string billIdOrReadingNumber, IAppUser appUser)
    {
        IEnumerable<int> myZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
        IEnumerable<ZoneIdAndCustomerNumberAndBillId> customerInfoByReadingNumber = await _commonMemberQueryService.GetFromClient(new ZoneIdsAndReadingNumber(myZoneIds, billIdOrReadingNumber), false);
        if ((customerInfoByReadingNumber?.Count() ?? 0) > 1)
        {
            throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThan1ReadingNumber);
        }

        if ((customerInfoByReadingNumber?.Count() ?? 0) == 1)
        {
            return customerInfoByReadingNumber.FirstOrDefault();
        }

        ZoneIdAndCustomerNumber customersZoneByBillId = await _commonMemberQueryService.Get(billIdOrReadingNumber);
        ZoneIdAndCustomerNumberAndBillId customerInfoByBillId = new(customersZoneByBillId.ZoneId, customersZoneByBillId.CustomerNumber, billIdOrReadingNumber);
        return customerInfoByBillId;
    }

    private async Task<(MeterReadingDetailDataOutputDto, MeterFlowStepEnum)> MeterReadingDetailLatestInfoByReadingNumberGetGetMeterReadingData(string billId)
    {
        MeterReadingDetailDataOutputDto? meterReadingDetail = await _meterReadingDetailQueryService.Get(billId);
        MeterFlowStepEnum meterFlowStepId = 0;
        if (meterReadingDetail is not null)
        {
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.GetLatestFlowInfo2(meterReadingDetail.FlowImportedId);
            if (meterFlowInfo.RemovedDateTime is not null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidLatestMeterReadingWithExpireMeterFlow);
            }

            meterFlowStepId = meterFlowInfo.MeterFlowStepId;
        }

        return (meterReadingDetail, meterFlowStepId);
    }

    private BillTransactionDetailWithLastReadingDataHeaderOutputDto MeterReadingDetailLatestInfoByReadingNumberGetGetHeader(BillTransactionDetailHeaderOutputDto preHeader, MeterReadingDetailDataOutputDto meterReadingDetail, MeterFlowStepEnum flowEnum)
    {
        MeterReadingDetailCheckedDto meterReadingWithCheck = _meterReadingCheckedListService.CheckedListGetGetReadingControlAsync(meterReadingDetail, flowEnum);
        return new BillTransactionDetailWithLastReadingDataHeaderOutputDto()
        {
            ZoneId = preHeader.ZoneId,
            ZoneTitle = preHeader.ZoneTitle,
            CustomerNumber = preHeader.CustomerNumber,
            BillId = preHeader.BillId,
            FirstName = preHeader.FirstName,
            Surname = preHeader.Surname,
            FullName = preHeader.FullName,
            Title = preHeader.Title,
            RecordCount = preHeader.RecordCount,
            LatestMeterChangeDateJalali = preHeader.LatestMeterChangeDateJalali,
            PreviousMeterDateJalali = preHeader.PreviousMeterDateJalali,
            PreviousMeterNumber = preHeader.PreviousMeterNumber,
            Id = meterReadingWithCheck.Id,
            CurrentMeterDeteJalali = meterReadingWithCheck.CurrentDateJalali,
            CurrentMeterNumber = meterReadingWithCheck.CurrentNumber,
            CurrentCounterStateCode = meterReadingWithCheck.CurrentCounterStateCode,
            UsageId = meterReadingWithCheck.UsageId,
            UsageTitle = meterReadingWithCheck.UsageTitle,
            ReadingNumber = meterReadingWithCheck.ReadingNumber,
            Amount = meterReadingWithCheck.SumItems ?? 0,
            CommercialUnit = meterReadingWithCheck.CommercialUnit,
            DomesticUnit = meterReadingWithCheck.DomesticUnit,
            OtherUnit = meterReadingWithCheck.OtherUnit,
            Consumption = meterReadingWithCheck.Consumption,
            ConsumptionAverage = meterReadingWithCheck.MonthlyConsumption,
            HasAttentionCounterState = meterReadingWithCheck.HasAttentionCounterState,
        };
    }
}
