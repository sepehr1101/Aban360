using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestWaterMeterDeleteHandler : IRequestWaterMeterDeleteHandler
    {
        private readonly IRequestWaterMeterCommandService _requestWaterMeterCommandService;
        private readonly IRequestWaterMeterQueryService _requestWaterMeterQueryService;
        public RequestWaterMeterDeleteHandler(
            IRequestWaterMeterCommandService requestWaterMeterCommandService,
            IRequestWaterMeterQueryService requestWaterMeterQueryService)
        {
            _requestWaterMeterCommandService = requestWaterMeterCommandService;
            _requestWaterMeterCommandService.NotNull(nameof(_requestWaterMeterCommandService));

            _requestWaterMeterQueryService = requestWaterMeterQueryService;
            _requestWaterMeterQueryService.NotNull(nameof(_requestWaterMeterQueryService));
        }

        public async Task Handle(WaterMeterRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestWaterMeter = await _requestWaterMeterQueryService.Get(deleteDto.Id);
            _requestWaterMeterCommandService.Remove(requestWaterMeter);
        }
    }
}
