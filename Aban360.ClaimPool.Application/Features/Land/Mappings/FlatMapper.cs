using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class FlatMapper:Profile
    {
        public FlatMapper()
        {
            CreateMap<FlatCreateDto, Flat>().ReverseMap();
            CreateMap<FlatDeleteDto, Flat>().ReverseMap();
            CreateMap<FlatUpdateDto, Flat>().ReverseMap();
            CreateMap<FlatGetDto, Flat>().ReverseMap();
        }
    }   
}
