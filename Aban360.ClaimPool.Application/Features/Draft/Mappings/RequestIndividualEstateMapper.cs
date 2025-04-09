using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestIndividualEstateMapper : Profile
    {
        public RequestIndividualEstateMapper()
        {
            CreateMap<IndividualEstateRequestCreateDto, RequestIndividualEstate>();
            CreateMap<IndividualEstateRequestDeleteDto, RequestIndividualEstate>();
            CreateMap<IndividualEstateRequestUpdateDto, RequestIndividualEstate>();
        }
    }
}
