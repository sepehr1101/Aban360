using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class UsageLevel1Mapper : Profile
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