using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class OfferingMapper : Profile
    {
        public OfferingMapper()
        {
            CreateMap<Offering, OfferingCreateDto>().ReverseMap();
            CreateMap<Offering, OfferingDeleteDto>().ReverseMap();
            CreateMap<Offering, OfferingUpdateDto>().ReverseMap();
            CreateMap<Offering, OfferingGetDto>()
                 .ForMember(dest => dest.OfferingGroupTitle, m => m.MapFrom(o => o.OfferingGroup.Title))
                 .ForMember(dest => dest.OfferingUnitTitle, m => m.MapFrom(o => o.OfferingUnit.Title));
        }
    }
}
