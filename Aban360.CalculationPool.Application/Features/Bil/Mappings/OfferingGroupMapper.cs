using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class OfferingGroupMapper : Profile
    {
        public OfferingGroupMapper()
        {
            CreateMap<OfferingGroup, OfferingGroupCreateDto>().ReverseMap();
            CreateMap<OfferingGroup, OfferingGroupDeleteDto>().ReverseMap();
            CreateMap<OfferingGroup, OfferingGroupUpdateDto>().ReverseMap();
            CreateMap<OfferingGroup, OfferingGroupGetDto>().ReverseMap();
        }
    }
}
