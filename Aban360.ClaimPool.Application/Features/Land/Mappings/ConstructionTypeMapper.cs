using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class ConstructionTypeMapper : Profile
    {
        public ConstructionTypeMapper()
        {
            CreateMap<ConstructionTypeCreateDto, ConstructionType>().ReverseMap();
            CreateMap<ConstructionTypeDeleteDto, ConstructionType>().ReverseMap();
            CreateMap<ConstructionTypeUpdateDto, ConstructionType>().ReverseMap();
            CreateMap<ConstructionTypeGetDto, ConstructionType>().ReverseMap();
        }
    }
   
}
