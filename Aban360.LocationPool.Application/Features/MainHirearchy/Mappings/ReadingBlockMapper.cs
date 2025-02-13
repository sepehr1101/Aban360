using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
{
    public class ReadingBlockMapper : Profile
    {
        public ReadingBlockMapper()
        {

            CreateMap<ReadingBlockCreateDto, ReadingBlock>()
                .ReverseMap();

            CreateMap<ReadingBlockDeleteDto, ReadingBlock>()
                .ReverseMap();
            
            CreateMap<ReadingBlockUpdateDto, ReadingBlock>()
                .ReverseMap();
            
            CreateMap<ReadingBlockGetDto, ReadingBlock>()
                .ReverseMap()
                .ForMember(dest => dest.ReadingBoundTitle, opt => opt.MapFrom(src => src.ReadingBound.Title));

        }
    }
}
