using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestWaterMeterTagDeleteHandler : IRequestWaterMeterTagDeleteHandler
    {
        private readonly IRequestWaterMeterTagCommandService _requestWaterMeterTagCommandService;
        private readonly IRequestWaterMeterTagQueryService _requestWaterMeterTagQueryService;
        public RequestWaterMeterTagDeleteHandler(
            IRequestWaterMeterTagCommandService requestWaterMeterTagCommandService,
            IRequestWaterMeterTagQueryService requestWaterMeterTagQueryService)
        {
            _requestWaterMeterTagCommandService = requestWaterMeterTagCommandService;
            _requestWaterMeterTagCommandService.NotNull(nameof(_requestWaterMeterTagCommandService));

            _requestWaterMeterTagQueryService = requestWaterMeterTagQueryService;
            _requestWaterMeterTagQueryService.NotNull(nameof(_requestWaterMeterTagQueryService));
        }

        public async Task Handle(WaterMeterTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestWaterMeterTag = await _requestWaterMeterTagQueryService.Get(deleteDto.Id);
            _requestWaterMeterTagCommandService.Remove(requestWaterMeterTag);
        }
    }
}
