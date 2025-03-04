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
            CreateMap<LineItemTypeCreateDto, LineItemType>().ReverseMap();
            CreateMap<LineItemTypeDeleteDto, LineItemType>().ReverseMap();
            CreateMap<LineItemTypeUpdateDto, LineItemType>().ReverseMap();
            CreateMap<LineItemTypeGetDto, LineItemType>().ReverseMap();
        }
    }
}
