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
            CreateMap<IndividualEstateCreateDto, IndividualEstate>().ReverseMap();
            CreateMap<IndividualEstateDeleteDto, IndividualEstate>().ReverseMap();
            CreateMap<IndividualEstateUpdateDto, IndividualEstate>().ReverseMap();
            CreateMap<IndividualEstateGetDto, IndividualEstate>().ReverseMap();
        }
    }
}
