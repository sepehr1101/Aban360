using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ReadingBlockMapper : Profile
    {
        public ReadingBlockMapper()
        {

            CreateMap<ReadingBlockCreateDto, ReadingBlock>();
            CreateMap<ReadingBlockDeleteDto, ReadingBlock>();
            CreateMap<ReadingBlockUpdateDto, ReadingBlock>();
            CreateMap<ReadingBlock,ReadingBlockGetDto>()
                .ForMember(dest => dest.ReadingBoundTitle, opt => opt.MapFrom(src => src.ReadingBound.Title));

        }
    }
}
