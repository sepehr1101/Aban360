using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestWaterMeterSiphonDeleteHandler : IRequestWaterMeterSiphonDeleteHandler
    {
        private readonly IRequestWaterMeterSiphonCommandService _requestWaterMeterSiphonCommandService;
        private readonly IRequestWaterMeterSiphonQueryService _requestWaterMeterSiphonQueryService;
        public RequestWaterMeterSiphonDeleteHandler(
            IRequestWaterMeterSiphonCommandService requestWaterMeterSiphonCommandService,
            IRequestWaterMeterSiphonQueryService requestWaterMeterSiphonQueryService)
        {
            _requestWaterMeterSiphonCommandService = requestWaterMeterSiphonCommandService;
            _requestWaterMeterSiphonCommandService.NotNull(nameof(_requestWaterMeterSiphonCommandService));

            _requestWaterMeterSiphonQueryService = requestWaterMeterSiphonQueryService;
            _requestWaterMeterSiphonQueryService.NotNull(nameof(_requestWaterMeterSiphonQueryService));
        }

        public async Task Handle(WaterMeterSiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var requestWaterMeterSiphon = await _requestWaterMeterSiphonQueryService.Get(deleteDto.Id);
            _requestWaterMeterSiphonCommandService.Remove(requestWaterMeterSiphon);
        }
    }
}
