using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class ImpactSignMapper : Profile
    {
        public ImpactSignMapper()
        {            
            CreateMap<ImpactSign, ImpactSignGetDto>();
        }
    }
}
