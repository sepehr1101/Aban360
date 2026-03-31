using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class CalculationRequestDisplayHandler : ICalculationRequestDisplayHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IKartQueryService _kartQueryService;
        static string _title = "مبالغ و تخفیفات";

        public CalculationRequestDisplayHandler(
            ITrackingQueryService trackingQueryService,
            IKartQueryService kartQueryService)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));
        }

        public async Task<ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            IEnumerable<CalculationRequestDisplayDataOutputDto> data = await _kartQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);

            IEnumerable<SaleAndAfterSaleDataOutputDto> resultData = data.Select(s => new SaleAndAfterSaleDataOutputDto((short)s.Id, s.Title, s.Amount + s.Discount, s.Discount, s.Amount, s.DiscountTypeId, s.Removable));
            long amount = resultData?.Sum(c => c.Amount) ?? 0;
            long discount = resultData?.Sum(c => c.Discount) ?? 0;
            SaleAndAfterSaleHeaderOutputDto header = new(amount, discount, amount - discount, resultData?.Count()??0);//todo: true or not

            return new ReportOutput<SaleAndAfterSaleHeaderOutputDto, SaleAndAfterSaleDataOutputDto>(_title, header, resultData);
        }
    }
}
