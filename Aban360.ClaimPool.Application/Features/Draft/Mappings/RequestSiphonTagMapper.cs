using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestSiphonMapper : Profile
    {
        public RequestSiphonMapper()
        {
            CreateMap<SiphonRequestCreateDto, RequestSiphon>();
            CreateMap<SiphonRequestDeleteDto, RequestSiphon>();
            CreateMap<SiphonRequestUpdateDto, RequestSiphon>();
        }
    }
}
