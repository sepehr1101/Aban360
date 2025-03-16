using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class ImpactSignMapper : Profile
    {
        public ImpactSignMapper()
        {
            CreateMap<ImpactSignCreateDto,ImpactSign >();
            CreateMap<ImpactSignDeleteDto,ImpactSign >();
            CreateMap<ImpactSignUpdateDto,ImpactSign >();
            CreateMap<ImpactSign, ImpactSignGetDto>();
        }
    }
}
