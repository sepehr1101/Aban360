using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class LineItemTypeMapper : Profile
    {
        public LineItemTypeMapper()
        {
            CreateMap<LineItemTypeCreateDto, LineItemType>();
            CreateMap<LineItemTypeDeleteDto, LineItemType>();
            CreateMap<LineItemTypeUpdateDto, LineItemType>();
            CreateMap<LineItemType, LineItemTypeGetDto>()
                    .ForMember(dest => dest.LineItemTypeGroupTitle, m => m.MapFrom(o => o.LineItemTypeGroup.Title));

        }
    }
}
