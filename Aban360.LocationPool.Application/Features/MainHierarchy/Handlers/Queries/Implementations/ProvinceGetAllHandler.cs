using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class ProvinceGetAllHandler : IProvinceGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IProvinceQueryService _provinceQueryService;
        public ProvinceGetAllHandler(
            IMapper mapper,
            IProvinceQueryService provinceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _provinceQueryService = provinceQueryService;
            _provinceQueryService.NotNull(nameof(provinceQueryService));
        }

        public async Task<ICollection<ProvinceGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Province> province = await _provinceQueryService.Get();
            return _mapper.Map<ICollection<ProvinceGetDto>>(province);
        }
    }
}
