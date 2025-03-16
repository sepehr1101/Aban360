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
            CreateMap<OfferingUnitCreateDto, OfferingUnit>();
            CreateMap<OfferingUnitDeleteDto,OfferingUnit >();
            CreateMap<OfferingUnitUpdateDto,OfferingUnit >();
            CreateMap<OfferingUnit, OfferingUnitGetDto>();
        }
    }
}
