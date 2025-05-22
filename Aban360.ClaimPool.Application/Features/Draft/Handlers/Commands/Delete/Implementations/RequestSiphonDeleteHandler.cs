using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Implementations
{
    internal sealed class RequestSiphonDeleteHandler : IRequestSiphonDeleteHandler
    {
        private readonly IRequestSiphonCommandService _requestSiphonCommandService;
        private readonly IRequestSiphonQueryService _requestSiphonQueryService;
        private readonly IRequestWaterMeterSiphonQueryService _requestWaterMeterSiphonQueryService;
        private readonly IRequestWaterMeterSiphonCommandService _requestWaterMeterSiphonCommandService;
        public RequestSiphonDeleteHandler(
            IRequestSiphonCommandService requestSiphonCommandService,
            IRequestSiphonQueryService requestSiphonQueryService,
            IRequestWaterMeterSiphonQueryService requestWaterMeterSiphonQueryService,
            IRequestWaterMeterSiphonCommandService requestWaterMeterSiphonCommandService)
        {
            _requestSiphonCommandService = requestSiphonCommandService;
            _requestSiphonCommandService.NotNull(nameof(_requestSiphonCommandService));

            _requestSiphonQueryService = requestSiphonQueryService;
            _requestSiphonQueryService.NotNull(nameof(_requestSiphonQueryService));

            _requestWaterMeterSiphonQueryService = requestWaterMeterSiphonQueryService;
            _requestWaterMeterSiphonQueryService.NotNull(nameof(_requestWaterMeterSiphonQueryService));

            _requestWaterMeterSiphonCommandService= requestWaterMeterSiphonCommandService;
            _requestWaterMeterSiphonCommandService.NotNull(nameof(_requestWaterMeterSiphonCommandService));
        }

        public async Task Handle(SiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            RequestWaterMeterSiphon requestWaterMeterSiphon = await _requestWaterMeterSiphonQueryService.GetBySiphonId(deleteDto.Id);
            if (requestWaterMeterSiphon != null)
            {
                _requestWaterMeterSiphonCommandService.Remove(requestWaterMeterSiphon);
            }
            var requestSiphon = await _requestSiphonQueryService.Get(deleteDto.Id);
            _requestSiphonCommandService.Remove(requestSiphon);
        }
    }
}
