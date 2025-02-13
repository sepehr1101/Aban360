using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using AutoMapper;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ReadingBoundMapper:Profile
    {
        public ReadingBoundMapper()
        {
            CreateMap<ReadingBoundCreateDto, ReadingBound>()
                .ReverseMap();

            CreateMap<ReadingBoundDeleteDto, ReadingBound>()
                .ReverseMap();

            CreateMap<ReadingBoundUpdateDto, ReadingBound>()
                .ReverseMap();

            CreateMap<ReadingBoundGetDto, ReadingBound>()
                .ReverseMap()
                .ForMember(dest => dest.ZoneTitle, opt => opt.MapFrom(src => src.Zone.Title));

        }
    }
}
