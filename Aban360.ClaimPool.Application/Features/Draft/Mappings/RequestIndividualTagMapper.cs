using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestIndividualTagMapper : Profile
    {
        public RequestIndividualTagMapper()
        {
            CreateMap<IndividualTagRequestCreateDto, RequestIndividualTag>();
            CreateMap<IndividualTagRequestDeleteDto, RequestIndividualTag>();
            CreateMap<IndividualTagRequestUpdateDto, RequestIndividualTag>();
        }
    }
}
