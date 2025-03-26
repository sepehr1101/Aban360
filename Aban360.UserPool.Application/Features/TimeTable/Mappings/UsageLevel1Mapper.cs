using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban360.UserPool.UsageLevel1lication.Features.TimeTable.MUsageLevel1ings
{
    public class UsageLevel1Mapper:Profile
    {

        public UsageLevel1Mapper()
        {
            CreateMap<UsageLevel1CreateDto, UsageLevel1>();
            CreateMap<UsageLevel1DeleteDto, UsageLevel1>();
            CreateMap<UsageLevel1UpdateDto, UsageLevel1>();
            CreateMap<UsageLevel1, UsageLevel1GetDto>();
        }
    }
}