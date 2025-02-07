using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ReadingBoundMapper:Profile
    {
        public ReadingBoundMapper()
        {
            CreateMap<ReadingBoundCreateDto, ReadingBound>().ReverseMap();
            CreateMap<ReadingBoundDeleteDto, ReadingBound>().ReverseMap();
            CreateMap<ReadingBoundUpdateDto, ReadingBound>().ReverseMap();
            CreateMap<ReadingBoundGetDto, ReadingBound>().ReverseMap();
        }
    }
}
