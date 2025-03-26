using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban460.UserPool.UsageLevel4lication.Features.TimeTable.MUsageLevel4ings
{
    public class UsageLevel4Mapper : Profile
    {

        public UsageLevel4Mapper()
        {
            CreateMap<UsageLevel4CreateDto, UsageLevel4>();
            CreateMap<UsageLevel4DeleteDto, UsageLevel4>();
            CreateMap<UsageLevel4UpdateDto, UsageLevel4>();

            CreateMap<UsageLevel4,UsageLevel4GetDto>()
                .ForMember(dest=>dest.UsageLevel3Title,x=>x.MapFrom(mem=> mem.UsageLevel3.Title));
        }
    }
}