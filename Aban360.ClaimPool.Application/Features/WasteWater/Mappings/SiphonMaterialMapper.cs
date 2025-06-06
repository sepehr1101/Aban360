﻿using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class SiphonMaterialMapper : Profile
    {
        public SiphonMaterialMapper()
        {
            CreateMap<SiphonMaterialCreateDto, SiphonMaterial>();
            CreateMap<SiphonMaterialDeleteDto, SiphonMaterial>();
            CreateMap<SiphonMaterialUpdateDto, SiphonMaterial>();
            CreateMap<SiphonMaterial,SiphonMaterialGetDto>();
        }
    }
}
