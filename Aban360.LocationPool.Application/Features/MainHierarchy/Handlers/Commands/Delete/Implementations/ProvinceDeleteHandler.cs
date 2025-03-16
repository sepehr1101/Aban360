using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class ProvinceDeleteHandler : IProvinceDeleteHandler
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
            Province province = await _provinceQueryService.Get(deleteDto.Id);
            await _provinceCommandService.Remove(province);
        }
    }
}
