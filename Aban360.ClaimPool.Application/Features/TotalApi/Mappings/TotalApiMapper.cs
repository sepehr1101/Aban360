using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Domain.Features.WasteWater.Base;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.TotalApi.Mappings
{
    public class TotalApiMapper : Profile
    {
        public TotalApiMapper()
        {
            CreateMap<EstateCreateDto, Estate>().ReverseMap();
            CreateMap<WaterMeterCreateDto, WaterMeter>().ReverseMap();
            CreateMap<SiphonCreateDto, Siphon>().ReverseMap();
            CreateMap<IndividualCreateDto, Individual>().ReverseMap();
        }
    }
}
