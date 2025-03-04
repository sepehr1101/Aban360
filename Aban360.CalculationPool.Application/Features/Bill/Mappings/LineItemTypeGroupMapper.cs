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
            CreateMap<LineItemTypeGroupCreateDto, LineItemTypeGroup>().ReverseMap();
            CreateMap<LineItemTypeGroupDeleteDto, LineItemTypeGroup>().ReverseMap();
            CreateMap<LineItemTypeGroupUpdateDto, LineItemTypeGroup>().ReverseMap();
            CreateMap<LineItemTypeGroupGetDto, LineItemTypeGroup>().ReverseMap();
        }
    }
}
