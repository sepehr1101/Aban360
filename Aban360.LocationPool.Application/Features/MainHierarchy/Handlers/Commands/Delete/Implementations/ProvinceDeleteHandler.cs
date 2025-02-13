using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    public class ProvinceDeleteHandler : IProvinceDeleteHandler
    {
        private readonly IProvinceCommandService _provinceCommandService;
        private readonly IProvinceQueryService _provinceQueryService;
        public ProvinceDeleteHandler(
            IProvinceCommandService provinceCommandService,
            IProvinceQueryService provinceQueryService)
        {
            _provinceCommandService = provinceCommandService;
            _provinceCommandService.NotNull(nameof(provinceCommandService));

            _provinceQueryService = provinceQueryService;
            _provinceQueryService.NotNull(nameof(provinceQueryService));
        }

        public async Task Handle(ProvinceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var province = await _provinceQueryService.Get(deleteDto.Id);
            if (province == null)
            {
                throw new InvalidDataException();
            }
            await _provinceCommandService.Remove(province);
        }
    }
}
