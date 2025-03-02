using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class ProfessionMapper : Profile
    {
        public ProfessionMapper()
        {
            CreateMap<ProfessionCreateDto, Profession>().ReverseMap();
            CreateMap<ProfessionDeleteDto, Profession>().ReverseMap();
            CreateMap<ProfessionUpdateDto, Profession>().ReverseMap();
            CreateMap<ProfessionGetDto, Profession>()
                .ReverseMap()
                .ForMember(dest => dest.GuildTitle, opt => opt.MapFrom(src => src.Guild.Title));
        }
    }
}
