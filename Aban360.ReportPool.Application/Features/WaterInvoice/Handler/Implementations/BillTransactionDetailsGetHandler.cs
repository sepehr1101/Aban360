using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Implementations
{
    internal sealed class BillTransactionDetailsGetHandler : IBillTransactionDetailsGetHandler
    {
        private readonly IBillQueryService _billQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ILatestWaterMeterInfoQueryService _latestWaterMeterInfoQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private int _firstMeterNumber = 1;
        public BillTransactionDetailsGetHandler(
            IBillQueryService billQueryService,
            IBedBesQueryService bedBesQueryService,
            ILatestWaterMeterInfoQueryService latestWaterMeterInfoQueryService,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService)
        {
            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _latestWaterMeterInfoQueryService = latestWaterMeterInfoQueryService;
            _latestWaterMeterInfoQueryService.NotNull(nameof(latestWaterMeterInfoQueryService));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<BillTransactionDetailGetDto> billDetails = await _billQueryService.GetBillDetails(billId);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            string? latestMeterChangeDateJalali = await _latestWaterMeterInfoQueryService.GetLatestChangeDateJalali(zoneIdAndCustomerNumber);

            string title = ReportLiterals.WaterInvoice;
            IEnumerable<BillTransactionDetailDataOutputDto> data = billDetails.Select(b => new BillTransactionDetailDataOutputDto()
            {
                UsageSellId = b.UsageSellId,
                UsageSellTitle = b.UsageSellTitle,
                UsageConsumptionId = b.UsageConsumptionId,
                UsageConsumptionTitle = b.UsageConsumptionTitle,
                BranchTypeTitle = b.BranchTypeTitle,
                BranchTypeId = b.BranchTypeId,
                PreviousDateJalali = b.PreviousDateJalali,
                CurrentDateJalali = b.CurrentDateJalali,
                RegisterDateJalali = b.RegisterDateJalali,
                DomesticUnit = b.DomesticUnit,
                CommercialUnit = b.CommercialUnit,
                OtherUnit = b.OtherUnit,
                PreviousNumber = b.PreviousNumber,
                NextNumber = b.NextNumber,
                Consumption = b.Consumption,
                ConsumptionAverage = b.ConsumptionAverage,
                SumItems = b.SumItems,
                EmptyCount = b.EmptyCount,
                CounterStateCode = b.CounterStateCode,
                CounterStateTitle = b.CounterStateTitle,

            });
            BedBesPreviousNumberAndDateOutputDto? bedBesPreviousNumberAndDate = await _bedBesQueryService.GetPreviousDateAndNumber(zoneIdAndCustomerNumber, billId, true);
            var (CurrentMeterDeteJalali, CurrentMeterNumber, CurrentCounterStateCode) = await GetMeterReadingData(billId);

            BillTransactionDetailHeaderOutputDto header = new()
            {
                ZoneId = memberInfo?.ZoneId ?? 0,
                ZoneTitle = memberInfo?.ZoneTitle ?? string.Empty,
                CustomerNumber = memberInfo?.CustomerNumber ?? 0,
                BillId = memberInfo?.BillId ?? string.Empty,
                Title = title,
                RecordCount = data?.Count() ?? 0,
                LatestMeterChangeDateJalali = latestMeterChangeDateJalali,
                PreviousMeterDateJalali = bedBesPreviousNumberAndDate is not null ? bedBesPreviousNumberAndDate.PreviousDateJalali : (memberInfo?.MeterInstallationDateJalali ?? string.Empty),
                PreviousMeterNumber = bedBesPreviousNumberAndDate is not null ? bedBesPreviousNumberAndDate.PreviousNumber : _firstMeterNumber,
                FirstName = memberInfo?.FirstName ?? string.Empty,
                Surname = memberInfo?.Surname ?? string.Empty,
                FullName = memberInfo?.FullName ?? string.Empty,

                CurrentMeterDeteJalali = CurrentMeterDeteJalali,
                CurrentMeterNumber = CurrentMeterNumber,
                CurrentCounterStateCode = CurrentCounterStateCode,
            };

            return new ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>(title, header, data);
        }
        private async Task<(string, int, short)> GetMeterReadingData(string billId)
        {
            MeterReadingDetailDataOutputDto meterReadingDetail = await _meterReadingDetailQueryService.Get(billId);
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.GetLatestFlowInfo2(meterReadingDetail.FlowImportedId);
            if (meterFlowInfo.RemovedDateTime is not null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidLatestMeterReadingWithExpireMeterFlow);
            }

            return (meterReadingDetail.CurrentDateJalali, meterReadingDetail.CurrentNumber, meterReadingDetail.CurrentCounterStateCode);
        }
    }
}
