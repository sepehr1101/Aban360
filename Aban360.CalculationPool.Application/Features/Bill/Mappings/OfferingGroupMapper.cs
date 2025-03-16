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
            CreateMap<OfferingGroupCreateDto, OfferingGroup>();
            CreateMap<OfferingGroupDeleteDto,OfferingGroup >();
            CreateMap<OfferingGroupUpdateDto,OfferingGroup >();
            CreateMap<OfferingGroup, OfferingGroupGetDto>();
        }
    }
}
