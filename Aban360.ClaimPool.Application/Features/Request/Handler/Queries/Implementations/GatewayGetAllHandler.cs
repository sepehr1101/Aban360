using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class GatewayGetAllHandler : IGatewayGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IGatewayQueryService _getewayQueryService;
        public GatewayGetAllHandler(
            IMapper mapper,
            IGatewayQueryService getewayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _getewayQueryService = getewayQueryService;
            _getewayQueryService.NotNull(nameof(_getewayQueryService));
        }

        public async Task<ICollection<GatewayGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Gateway> geteway = await _getewayQueryService.Get();
            return _mapper.Map<ICollection<GatewayGetDto>>(geteway);
        }
    }
}