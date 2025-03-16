using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class LineItemTypeGroupMapper : Profile
    {
        public LineItemTypeGroupMapper()
        {
            CreateMap<LineItemTypeGroupCreateDto, LineItemTypeGroup>();
            CreateMap<LineItemTypeGroupDeleteDto, LineItemTypeGroup>();
            CreateMap<LineItemTypeGroupUpdateDto, LineItemTypeGroup>();
            CreateMap< LineItemTypeGroup, LineItemTypeGroupGetDto>()
                                .ForMember(dest => dest.ImpactSignTitle, m => m.MapFrom(o => o.ImpactSign.Title));

        }
    }
}
