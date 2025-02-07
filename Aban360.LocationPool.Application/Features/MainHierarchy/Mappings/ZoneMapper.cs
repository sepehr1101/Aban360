﻿using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ZoneMapper:Profile
    {
        public ZoneMapper()
        {
            CreateMap<ZoneCreateDto, Zone>().ReverseMap();
            CreateMap<ZoneDeleteDto, Zone>().ReverseMap();
            CreateMap<ZoneUpdateDto, Zone>().ReverseMap();
            CreateMap<ZoneGetDto, Zone>().ReverseMap();
        }
    }
}
