using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using NetTopologySuite.Index.HPRtree;

namespace Aban360.ReportPool.Application.Features.WaterInvoice.Handler.Implementations
{
    internal sealed class BillLatestListGetHandler : IBillLatestListGetHandler
    {
        private readonly IBillQueryService _billQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private string _title = ReportLiterals.WaterLatestList;
        public BillLatestListGetHandler(
            IBillQueryService billQueryService,
            IBedBesQueryService bedBesQueryService,
            ICommonZoneService commonZoneService)
        {
            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto>> Handle(BillLatestListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            IEnumerable<BillLatestListDataOutputDto> data = await _billQueryService.GetLatestList(inputDto);
            foreach (var item in data)
            {
                BedBesPreviousNumberAndDateOutputDto billInfo = await _bedBesQueryService.GetPreviousDateAndNumber(new ZoneIdAndCustomerNumber(item.ZoneId, item.CustomerNumber), item.BillId);
                item.PreviousNumber = billInfo.PreviousNumber;
                item.PreviousDateJalali = billInfo.PreviousDateJalali;
            }
            BillLatestListHeaderOutputDto header = new()
            {
                ZoneId = inputDto.ZoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                FromReadingNumber = inputDto.FromReadingNumber,
                ToReadingNumber = inputDto.ToReadingNumber,
                RecordCount = data?.Count() ?? 0,
                Title = _title,
            };
            ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto> result = new(_title, header, data);
            return result;
        }
        public async Task<ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto>> HandleByBedBes(BillLatestListInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            IEnumerable<BillLatestListDataOutputDto> data = await _billQueryService.GetLatestListByBedBes(inputDto);
        
            foreach (var item in data)
            {
                if (item.IsReturned)
                {
                    BedBesPreviousNumberAndDateOutputDto billInfo = await _bedBesQueryService.GetPreviousDateAndNumber(new ZoneIdAndCustomerNumber(item.ZoneId, item.CustomerNumber), item.BillId);
                    item.PreviousNumber = billInfo.PreviousNumber;
                    item.PreviousDateJalali = billInfo.PreviousDateJalali;
                }
            }
            BillLatestListHeaderOutputDto header = new()
            {
                ZoneId = inputDto.ZoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                FromReadingNumber = inputDto.FromReadingNumber,
                ToReadingNumber = inputDto.ToReadingNumber,
                RecordCount = data?.Count() ?? 0,
                Title = _title,
            };
            ReportOutput<BillLatestListHeaderOutputDto, BillLatestListDataOutputDto> result = new(_title, header, data);
            return result;
        }
    }
}
