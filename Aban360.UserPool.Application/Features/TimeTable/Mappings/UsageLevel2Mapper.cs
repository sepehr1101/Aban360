using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban360.UserPool.UsageLevel2lication.Features.TimeTable.MUsageLevel2ings
{
    public class UsageLevel2Mapper : Profile
    {

        public UsageLevel2Mapper()
        {
            CreateMap<UsageLevel2CreateDto, UsageLevel2>();
            CreateMap<UsageLevel2DeleteDto, UsageLevel2>();
            CreateMap<UsageLevel2UpdateDto, UsageLevel2>();

            CreateMap<UsageLevel2,UsageLevel2GetDto>()
                .ForMember(dest=>dest.UsageLevel1Title,x=>x.MapFrom(mem=> mem.UsageLevel1.Title));
        }
    }
}