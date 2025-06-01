using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Implementations
{
    internal sealed class RequestTrackingGetAllHandler : IRequestTrackingGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestTrackingQueryService _requestTrackingQueryService;
        public RequestTrackingGetAllHandler(
            IMapper mapper,
            IRequestTrackingQueryService requestTrackingQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestTrackingQueryService = requestTrackingQueryService;
            _requestTrackingQueryService.NotNull(nameof(_requestTrackingQueryService));
        }

        public async Task<ICollection<RequestTrackingGetDto>> Handle(CancellationToken cancellationToken)
        {
            var requestTracking = await _requestTrackingQueryService.Get();
            return _mapper.Map<ICollection<RequestTrackingGetDto>>(requestTracking);
        }
    }
}
