using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestWaterMeterDeleteHandler : IRequestWaterMeterDeleteHandler
    {
        private readonly IRequestWaterMeterCommandService _requestWaterMeterCommandService;
        private readonly IRequestWaterMeterQueryService _requestWaterMeterQueryService;
        private readonly IRequestWaterMeterTagCommandService _requestWaterMeterTagCommandService;
        private readonly IRequestWaterMeterTagQueryService _requestWaterMeterTagQueryService;
        public RequestWaterMeterDeleteHandler(
            IRequestWaterMeterCommandService requestWaterMeterCommandService,
            IRequestWaterMeterQueryService requestWaterMeterQueryService,
            IRequestWaterMeterTagCommandService requestWaterMeterTagCommandService,
            IRequestWaterMeterTagQueryService requestWaterMeterTagQueryService)
        {
            _requestWaterMeterCommandService = requestWaterMeterCommandService;
            _requestWaterMeterCommandService.NotNull(nameof(_requestWaterMeterCommandService));

            _requestWaterMeterQueryService = requestWaterMeterQueryService;
            _requestWaterMeterQueryService.NotNull(nameof(_requestWaterMeterQueryService));

            _requestWaterMeterTagCommandService = requestWaterMeterTagCommandService;
            _requestWaterMeterTagCommandService.NotNull(nameof(_requestWaterMeterTagCommandService));

            _requestWaterMeterTagQueryService = requestWaterMeterTagQueryService;
            _requestWaterMeterTagQueryService.NotNull(nameof(_requestWaterMeterTagQueryService));
        }

        public async Task Handle(WaterMeterRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            ICollection<RequestWaterMeterTag> requestWaterMeterTags = await _requestWaterMeterTagQueryService.GetByWaterMeterId(deleteDto.Id);
            if (requestWaterMeterTags != null)
            {
                _requestWaterMeterTagCommandService.Remove(requestWaterMeterTags);
            }

            var requestWaterMeter = await _requestWaterMeterQueryService.Get(deleteDto.Id);
            _requestWaterMeterCommandService.Remove(requestWaterMeter);
        }
    }
}
