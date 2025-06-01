using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestTrackingUpdateHandler : IRequestTrackingUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestTrackingQueryService _requestTrackingQueryService;
        public RequestTrackingUpdateHandler(
            IMapper mapper,
            IRequestTrackingQueryService requestTrackingQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestTrackingQueryService = requestTrackingQueryService;
            _requestTrackingQueryService.NotNull(nameof(_requestTrackingQueryService));
        }

        public async Task Handle(RequestTrackingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestTracking = await _requestTrackingQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestTracking);
        }
    }
}
