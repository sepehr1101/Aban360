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
            CreateMap<OfferingCreateDto, Offering>();
            CreateMap<OfferingDeleteDto,Offering>();
            CreateMap<OfferingUpdateDto,Offering>();
            CreateMap<Offering, OfferingGetDto>()
                 .ForMember(dest => dest.OfferingGroupTitle, m => m.MapFrom(o => o.OfferingGroup.Title))
                 .ForMember(dest => dest.OfferingUnitTitle, m => m.MapFrom(o => o.OfferingUnit.Title));
        }
    }
}
