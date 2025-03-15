using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class GetewayGetAllHandler : IGetewayGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IGetewayQueryService _getewayQueryService;
        public GetewayGetAllHandler(
            IMapper mapper,
            IGetewayQueryService getewayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _getewayQueryService = getewayQueryService;
            _getewayQueryService.NotNull(nameof(_getewayQueryService));
        }

        public async Task<ICollection<GetewayGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Geteway> geteway = await _getewayQueryService.Get();
            return _mapper.Map<ICollection<GetewayGetDto>>(geteway);
        }
    }
}