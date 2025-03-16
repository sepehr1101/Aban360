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
            CreateMap<FlatCreateDto, Flat>();
            CreateMap<FlatDeleteDto, Flat>();
            CreateMap<FlatUpdateDto, Flat>();
            CreateMap<Flat, FlatGetDto>()
                .ForMember(dest => dest.EstateTitle, x => x.MapFrom(mem => mem.Estate.PostalCode));
        }
    }   
}
