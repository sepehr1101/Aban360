using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class UsageLevel2Mapper : Profile
    {

        public UsageLevel2Mapper()
        {
            CreateMap<UsageLevel2CreateDto, UsageLevel2>();
            CreateMap<UsageLevel2DeleteDto, UsageLevel2>();
            CreateMap<UsageLevel2UpdateDto, UsageLevel2>();

            CreateMap<UsageLevel2, UsageLevel2GetDto>()
                .ForMember(dest => dest.UsageLevel1Title, x => x.MapFrom(mem => mem.UsageLevel1.Title));
        }
    }
}