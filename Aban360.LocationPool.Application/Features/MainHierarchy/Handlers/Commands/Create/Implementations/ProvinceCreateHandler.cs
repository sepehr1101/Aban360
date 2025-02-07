using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    public class ProvinceCreateHandler : IProvinceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IProvinceCommandService _provinceCommandService;
        public ProvinceCreateHandler(
            IMapper mapper,
            IProvinceCommandService provinceCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _provinceCommandService = provinceCommandService;
            _provinceCommandService.NotNull(nameof(provinceCommandService));
        }

        public async Task Handle(ProvinceCreateDto createDto, CancellationToken cancellationToken)
        {
            var province = _mapper.Map<Province>(createDto);
            await _provinceCommandService.Add(province);
        }
    }
}
