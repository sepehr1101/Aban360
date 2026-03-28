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

        public async Task<ReportOutput<CalculationRequestDisplayHeaderOutputDto, CalculationRequestDisplayDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            IEnumerable<CalculationRequestDisplayDataOutputDto> data = await _kartQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);

            long amount = data?.Sum(c => c.Amount) ?? 0;
            long discount = data?.Sum(c => c.Discount ) ?? 0;
            CalculationRequestDisplayHeaderOutputDto header = new(amount, discount, amount - discount);//todo: true or not

            return new ReportOutput<CalculationRequestDisplayHeaderOutputDto, CalculationRequestDisplayDataOutputDto>(_title, header, data);
        }
    }
}
