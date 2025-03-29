using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualEstateMapper : Profile
    {
        public IndividualEstateMapper()
        {
            CreateMap<IndividualEstateCreateDto, IndividualEstate>();
            CreateMap<IndividualEstateDeleteDto, IndividualEstate>();
            CreateMap<IndividualEstateUpdateDto, IndividualEstate>();
            CreateMap<IndividualEstate,IndividualEstateGetDto>();
        }
    }
}
