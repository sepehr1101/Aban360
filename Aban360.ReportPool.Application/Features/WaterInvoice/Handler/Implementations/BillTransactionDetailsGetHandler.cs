using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
        private readonly ILatestWaterMeterInfoQueryService _latestWaterMeterInfoQueryService;
        public BillTransactionDetailsGetHandler(
            IBillQueryService billQueryService,
            ILatestWaterMeterInfoQueryService latestWaterMeterInfoQueryService)
        {
            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _latestWaterMeterInfoQueryService = latestWaterMeterInfoQueryService;
            _latestWaterMeterInfoQueryService.NotNull(nameof(latestWaterMeterInfoQueryService));
        }

        public async Task<ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            IEnumerable<BillTransactionDetailGetDto> billDetails = await _billQueryService.GetBillDetails(billId);
            string? latestMeterChangeDateJalali = await _latestWaterMeterInfoQueryService.GetLatestChangeDateJalali(new ZoneIdAndCustomerNumber(billDetails?.FirstOrDefault()?.ZoneId ?? 0, billDetails?.FirstOrDefault()?.CustomerNumber ?? 0));

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
                CommertialUnit = b.CommertialUnit,
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
            BillTransactionDetailHeaderOutputDto header = new()
            {
                ZoneId = billDetails?.FirstOrDefault()?.ZoneId ?? 0,
                ZoneTitle = billDetails?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                CustomerNumber = billDetails?.FirstOrDefault()?.CustomerNumber ?? 0,
                BillId = billDetails?.FirstOrDefault()?.BillId ?? string.Empty,
                Title = title,
                RecordCount = data?.Count() ?? 0,
                LatestMeterChangeDateJalali = latestMeterChangeDateJalali
            };

            return new ReportOutput<BillTransactionDetailHeaderOutputDto, BillTransactionDetailDataOutputDto>(title, header, data);
        }
    }
}
