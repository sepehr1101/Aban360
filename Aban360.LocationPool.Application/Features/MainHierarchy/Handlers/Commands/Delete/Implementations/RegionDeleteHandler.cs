﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Implementations
{
    internal sealed class RegionDeleteHandler : IRegionDeleteHandler
    {
        private readonly IRegionQueryService _regionQueryService;
        private readonly IRegionCommandService _regionCommandService;
        public RegionDeleteHandler(
            IRegionQueryService regionQueryService,
            IRegionCommandService regionCommandService)
        {
            _regionQueryService = regionQueryService;
            _regionQueryService.NotNull(nameof(regionQueryService));

            _regionCommandService = regionCommandService;
            _regionCommandService.NotNull(nameof(regionCommandService));
        }

        public async Task Handle(RegionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            Region region = await _regionQueryService.Get(deleteDto.Id);
            await _regionCommandService.Remove(region);
        }
    }
}
