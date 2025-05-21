using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestIndividualMapper : Profile
    {
        public RequestIndividualMapper()
        {
            CreateMap<IndividualRequestCreateDto, RequestIndividual>();
            CreateMap<IndividualRequestDeleteDto, RequestIndividual>();
            CreateMap<IndividualRequestUpdateDto, RequestIndividual>();
        }
    }
}
