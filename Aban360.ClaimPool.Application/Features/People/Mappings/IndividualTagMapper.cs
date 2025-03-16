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
            CreateMap<IndividualTagCreateDto, IndividualTag>();
            CreateMap<IndividualTagDeleteDto, IndividualTag>();
            CreateMap<IndividualTagUpdateDto, IndividualTag>();
            CreateMap<IndividualTag,IndividualTagGetDto>();
        }
    }
}
