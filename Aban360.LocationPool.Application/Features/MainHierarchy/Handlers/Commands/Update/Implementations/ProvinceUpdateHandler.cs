using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    public class ProvinceUpdateHandler : IProvinceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IProvinceQueryService _provinceQueryService;
        public ProvinceUpdateHandler(
            IMapper mapper,
            IProvinceQueryService provinceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _provinceQueryService = provinceQueryService;
            _provinceQueryService.NotNull(nameof(provinceQueryService));
        }

        public async Task Handle(ProvinceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var province = await _provinceQueryService.Get(updateDto.Id);
            if (province == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, province);
        }
    }
}
