using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class EstateMapper : Profile
    {
        public EstateMapper()
        {
            CreateMap<EstateCreateDto, Estate>().ReverseMap();
            CreateMap<EstateDeleteDto, Estate>().ReverseMap();
            CreateMap<EstateUpdateDto, Estate>().ReverseMap();
            CreateMap<EstateGetDto, Estate>().ReverseMap();
        }
    }
   
}
