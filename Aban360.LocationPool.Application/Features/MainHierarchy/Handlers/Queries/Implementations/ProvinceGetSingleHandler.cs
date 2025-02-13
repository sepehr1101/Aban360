using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public class ProvinceGetSingleHandler : IProvinceGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IProvinceQueryService _provinceQueryService;
        public ProvinceGetSingleHandler(
            IMapper mapper,
            IProvinceQueryService provinceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _provinceQueryService = provinceQueryService;
            _provinceQueryService.NotNull(nameof(provinceQueryService));
        }
        public async Task<ProvinceGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var province = await _provinceQueryService.Get(id);
            return _mapper.Map<ProvinceGetDto>(province);
        }
    }
}
