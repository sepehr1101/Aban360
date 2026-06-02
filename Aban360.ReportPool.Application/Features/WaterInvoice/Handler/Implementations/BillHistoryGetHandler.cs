using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Implementations
{
    internal sealed class BillHistoryGetHandler : IBillHistoryGetHandler
    {
        private readonly IBillQueryService _billQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private string _title = ReportLiterals.BillsHistory;
        public BillHistoryGetHandler(
            IBillQueryService billQueryService,
            ICommonZoneService commonZoneService)
        {
            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<BillHistoryHeaderOutputDto, BillHistoryDataOutputDto>> Handle(BillHistoryInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<BillHistoryDataOutputDto> data = await _billQueryService.GetHistory(input);
            await _commonZoneService.IsUserInZone(appUser, data?.FirstOrDefault()?.ZoneId ?? 0);
            BillHistoryHeaderOutputDto header = new()
            {
                BillId = input.BillId,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                Title = _title,
                RecordCount = data?.Count() ?? 0,

                Payable = data?.Sum(d => d.Payable) ?? 0,
                SumItems = data?.Sum(d => d.SumItems) ?? 0,
                AbBaha = data?.Sum(d => d.AbBaha) ?? 0,
                FazelabBaha = data?.Sum(d => d.FazelabBaha) ?? 0,
                AbonmanAb = data?.Sum(d => d.AbonmanAb) ?? 0,
                AbonmanFazelab = data?.Sum(d => d.AbonmanFazelab) ?? 0,
                Maliat = data?.Sum(d => d.Maliat) ?? 0,
                Tabsare2 = data?.Sum(d => d.Tabsare2) ?? 0,
                Tabsare2_3 = data?.Sum(d => d.Tabsare2_3) ?? 0,
                Jarime = data?.Sum(d => d.Jarime) ?? 0,
                Abresani = data?.Sum(d => d.Abresani) ?? 0,
                JavaniJamiat = data?.Sum(d => d.JavaniJamiat) ?? 0,
                FaslGarm = data?.Sum(d => d.FaslGarm) ?? 0,
                ZaribTadil = data?.Sum(d => d.ZaribTadil) ?? 0,
                Tabsare3Ab = data?.Sum(d => d.Tabsare3Ab) ?? 0,
                Tabsare3Fazelab = data?.Sum(d => d.Tabsare3Fazelab) ?? 0,
                TabsareAbonmanFazelab = data?.Sum(d => d.TabsareAbonmanFazelab) ?? 0,
                GhanonBoodje = data?.Sum(d => d.GhanonBoodje) ?? 0,
                JavazemKahande = data?.Sum(d => d.JavazemKahande) ?? 0,
                AvarezSanati = data?.Sum(d => d.AvarezSanati) ?? 0,
            };
            return new ReportOutput<BillHistoryHeaderOutputDto, BillHistoryDataOutputDto>(_title, header, data);
        }
    }
}
