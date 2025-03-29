using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class UsageLevel4Mapper : Profile
    {

        public UsageLevel4Mapper()
        {
            CreateMap<UsageLevel4CreateDto, UsageLevel4>();
            CreateMap<UsageLevel4DeleteDto, UsageLevel4>();
            CreateMap<UsageLevel4UpdateDto, UsageLevel4>();

            CreateMap<UsageLevel4, UsageLevel4GetDto>()
                .ForMember(dest => dest.UsageLevel3Title, x => x.MapFrom(mem => mem.UsageLevel3.Title));
        }
    }
}