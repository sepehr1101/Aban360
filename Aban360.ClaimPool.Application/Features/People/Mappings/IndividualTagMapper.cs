using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualTagMapper : Profile
    {
        public IndividualTagMapper()
        {
            CreateMap<IndividualTagCreateDto, IndividualTag>().ReverseMap();
            CreateMap<IndividualTagDeleteDto, IndividualTag>().ReverseMap();
            CreateMap<IndividualTagUpdateDto, IndividualTag>().ReverseMap();
            CreateMap<IndividualTagGetDto, IndividualTag>().ReverseMap();
        }
    }
}
