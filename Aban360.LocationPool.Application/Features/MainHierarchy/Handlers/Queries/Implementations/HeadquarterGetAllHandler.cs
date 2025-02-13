using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public class HeadquarterGetAllHandler : IHeadquarterGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IHeadquarterQueryService _headquarterQueryService;
        public HeadquarterGetAllHandler(
            IMapper mapper,
            IHeadquarterQueryService headquarterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _headquarterQueryService = headquarterQueryService;
            _headquarterQueryService.NotNull(nameof(headquarterQueryService));
        }

        public async Task<ICollection<HeadquarterGetDto>> Handle(CancellationToken cancellationToken)
        {
            var headquarter = await _headquarterQueryService.Get();
            return _mapper.Map<ICollection<HeadquarterGetDto>>(headquarter);
        }
    }
}
