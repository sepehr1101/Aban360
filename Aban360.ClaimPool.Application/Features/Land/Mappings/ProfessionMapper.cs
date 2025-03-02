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
            CreateMap<ProfessionCreateDto, Profession>();
            CreateMap<ProfessionDeleteDto, Profession>();
            CreateMap<ProfessionUpdateDto, Profession>();
            CreateMap<Profession,ProfessionGetDto>()
                .ForMember(dest => dest.GuildTitle, opt => opt.MapFrom(src => src.Guild.Title));
        }
    }
}
