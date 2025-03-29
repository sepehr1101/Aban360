using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class UsageLevel3Mapper : Profile
    {

        public UsageLevel3Mapper()
        {
            CreateMap<UsageLevel3CreateDto, UsageLevel3>();
            CreateMap<UsageLevel3DeleteDto, UsageLevel3>();
            CreateMap<UsageLevel3UpdateDto, UsageLevel3>();

            CreateMap<UsageLevel3, UsageLevel3GetDto>()
                .ForMember(dest => dest.UsageLevel2Title, x => x.MapFrom(mem => mem.UsageLevel2.Title));
        }
    }
}