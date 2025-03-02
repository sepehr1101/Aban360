using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class GuildMapper : Profile
    {
        public GuildMapper()
        {
            CreateMap<GuildCreateDto, Guild>();
            CreateMap<GuildDeleteDto, Guild>();
            CreateMap<GuildUpdateDto, Guild>();
            CreateMap<Guild, GuildGetDto>()
                .ForMember(dest => dest.UsageTitle, opt => opt.MapFrom(src => src.Usage.Title));
        }
    }
}
