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
            CreateMap<IndividualEstateRelationTypeCreateDto, IndividualEstateRelationType>().ReverseMap();
            CreateMap<IndividualEstateRelationTypeDeleteDto, IndividualEstateRelationType>().ReverseMap();
            CreateMap<IndividualEstateRelationTypeUpdateDto, IndividualEstateRelationType>().ReverseMap();
            CreateMap<IndividualEstateRelationTypeGetDto, IndividualEstateRelationType>().ReverseMap();
        }
    }
}
