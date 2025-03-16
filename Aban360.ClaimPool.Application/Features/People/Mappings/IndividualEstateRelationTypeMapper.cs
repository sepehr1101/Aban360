using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualEstateRelationTypeMapper : Profile
    {
        public IndividualEstateRelationTypeMapper()
        {
            CreateMap<IndividualEstateRelationTypeCreateDto, IndividualEstateRelationType>();
            CreateMap<IndividualEstateRelationTypeDeleteDto, IndividualEstateRelationType>();
            CreateMap<IndividualEstateRelationTypeUpdateDto, IndividualEstateRelationType>();
            CreateMap< IndividualEstateRelationType,IndividualEstateRelationTypeGetDto > ();
        }
    }
}
