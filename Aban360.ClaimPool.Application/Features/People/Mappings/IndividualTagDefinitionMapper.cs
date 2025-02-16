using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualTagDefinitionMapper : Profile
    {
        public IndividualTagDefinitionMapper()
        {
            CreateMap<IndividualTagDefinitionCreateDto, IndividualTagDefinition>().ReverseMap();
            CreateMap<IndividualTagDefinitionDeleteDto, IndividualTagDefinition>().ReverseMap();
            CreateMap<IndividualTagDefinitionUpdateDto, IndividualTagDefinition>().ReverseMap();
            CreateMap<IndividualTagDefinitionGetDto, IndividualTagDefinition>().ReverseMap();
        }
    }
}
