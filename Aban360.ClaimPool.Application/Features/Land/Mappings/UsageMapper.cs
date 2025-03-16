using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class UsageMapper : Profile
    {
        public UsageMapper()
        {
            CreateMap<UsageCreateDto, Usage>();
            CreateMap<UsageDeleteDto, Usage>();
            CreateMap<UsageUpdateDto, Usage>();
            CreateMap<Usage,UsageGetDto>();
        }
    }

}

