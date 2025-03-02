using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class OfferingUnitMapper : Profile
    {
        public OfferingUnitMapper()
        {
            CreateMap<OfferingUnit, OfferingUnitCreateDto>().ReverseMap();
            CreateMap<OfferingUnit, OfferingUnitDeleteDto>().ReverseMap();
            CreateMap<OfferingUnit, OfferingUnitUpdateDto>().ReverseMap();
            CreateMap<OfferingUnit, OfferingUnitGetDto>().ReverseMap();
        }
    }
}
