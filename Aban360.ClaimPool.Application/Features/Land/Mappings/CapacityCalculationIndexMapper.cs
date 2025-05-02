using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class CapacityCalculationIndexMapper : Profile
    {
        public CapacityCalculationIndexMapper()
        {
            CreateMap<CapacityCalculationIndexCreateDto, CapacityCalculationIndex>();
            CreateMap<CapacityCalculationIndexDeleteDto, CapacityCalculationIndex>();
            CreateMap<CapacityCalculationIndexUpdateDto, CapacityCalculationIndex>();
            CreateMap<CapacityCalculationIndex,CapacityCalculationIndexGetDto>();
        }
    }   
}
