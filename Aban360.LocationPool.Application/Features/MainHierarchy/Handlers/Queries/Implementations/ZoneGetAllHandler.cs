﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class ZoneGetAllHandler: IZoneGetAllHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        private readonly IMapper _mapper;
        public ZoneGetAllHandler(
           IZoneQueryService zoneQueryService,
            IMapper mapper)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }

        public async Task<ICollection<ZoneGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Zone> zone = await _zoneQueryService.Get();
            return _mapper.Map<ICollection<ZoneGetDto>>(zone);
        }
    }
}
