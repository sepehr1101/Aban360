using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Mappings
{
    public class IndividualMapper : Profile
    {
        public IndividualMapper()
        {
            CreateMap<IndividualCreateDto, Individual>();
            CreateMap<IndividualDeleteDto, Individual>();
            CreateMap<IndividualUpdateDto, Individual>();
            CreateMap<Individual,IndividualGetDto>();
        }
    }
}
