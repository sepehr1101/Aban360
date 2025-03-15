using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class HeadquarterGetSingleHandler : IHeadquarterGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IHeadquarterQueryService _headquarterQueryService;
        public HeadquarterGetSingleHandler(
            IMapper mapper,
            IHeadquarterQueryService headquarterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _headquarterQueryService = headquarterQueryService;
            _headquarterQueryService.NotNull(nameof(headquarterQueryService));
        }
  
        public async Task<HeadquarterGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            Headquarters headquarter = await _headquarterQueryService.Get(id);
            return _mapper.Map<HeadquarterGetDto>(headquarter);
        }
    }
}
