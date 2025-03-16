using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualTagDefinitionMapper : Profile
    {
        public IndividualTagDefinitionMapper()
        {
            CreateMap<IndividualTagDefinitionCreateDto, IndividualTagDefinition>();
            CreateMap<IndividualTagDefinitionDeleteDto, IndividualTagDefinition>();
            CreateMap<IndividualTagDefinitionUpdateDto, IndividualTagDefinition>();
            CreateMap<IndividualTagDefinition,IndividualTagDefinitionGetDto>();
        }
    }
}
