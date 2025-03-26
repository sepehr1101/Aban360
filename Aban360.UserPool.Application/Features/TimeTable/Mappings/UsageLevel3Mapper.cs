using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban360.UserPool.UsageLevel3lication.Features.TimeTable.MUsageLevel3ings
{
    public class UsageLevel3Mapper : Profile
    {

        public UsageLevel3Mapper()
        {
            CreateMap<UsageLevel3CreateDto, UsageLevel3>();
            CreateMap<UsageLevel3DeleteDto, UsageLevel3>();
            CreateMap<UsageLevel3UpdateDto, UsageLevel3>();

            CreateMap<UsageLevel3,UsageLevel3GetDto>()
                .ForMember(dest=>dest.UsageLevel2Title,x=>x.MapFrom(mem=> mem.UsageLevel2.Title));
        }
    }
}