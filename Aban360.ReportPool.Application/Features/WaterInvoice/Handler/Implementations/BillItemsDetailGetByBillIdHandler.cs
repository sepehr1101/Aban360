using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Implementations
{
    internal sealed class BillItemsDetailGetByBillIdHandler : IBillItemsDetailGetByBillIdHandler
    {
        private readonly IBillQueryService _billQueryService;
        public BillItemsDetailGetByBillIdHandler(IBillQueryService billQueryService)
        {
            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }

        public async Task<FlatReportOutput<BillItemsHeaderOutputDto, BillItemsDataOutputDto>> Handle(int billId, CancellationToken cancellationToken)
        {
            string title = ReportLiterals.WaterItemsInvoice;
            BillItemsGetDto billItems = await _billQueryService.Get(billId);
            BillItemsDataOutputDto data = new()
            {
                AbBaha = billItems.AbBaha,
                FazelabBaha = billItems.FazelabBaha,
                AbonmanAb = billItems.AbonmanAb,
                AbonmanFazelab = billItems.AbonmanFazelab,
                Maliat = billItems.Maliat,
                Tabsare2 = billItems.Tabsare2,
                Tabsare2_3 = billItems.Tabsare2_3,
                Jarime = billItems.Jarime,
                Abresani = billItems.Abresani,
                JavaniJamiat = billItems.JavaniJamiat,
                FaslGarm = billItems.FaslGarm,
                ZaribTadil = billItems.ZaribTadil,
                Tabsare3Ab = billItems.Tabsare3Ab,
                Tabsare3Fazelab = billItems.Tabsare3Fazelab,
                TabsareAbonmanFazelab = billItems.TabsareAbonmanFazelab,
                GhanonBoodje = billItems.GhanonBoodje,
                JavazemKahande = billItems.JavazemKahande,
                Boodje = billItems.Boodje,
            };
            BillItemsHeaderOutputDto header = new()
            {
                BillId = billItems.BillId,
                Id = billItems.Id,
                CustomerNumber = billItems.CustomerNumber,
                RegionId = billItems.RegionId,
                RegionTitle = billItems.RegionTitle,
                ZoneId = billItems.ZoneId,
                ZoneTitle = billItems.ZoneTitle,
                UsageId = billItems.UsageId,
                UsageTitle = billItems.UsageTitle,
                BranchTypeId = billItems.BranchTypeId,
                BranchTypeTitle = billItems.BranchTypeTitle,
                Title = title
            };
            FlatReportOutput<BillItemsHeaderOutputDto, BillItemsDataOutputDto> result = new(title, header, data);
            return result;
        }
    }
}
