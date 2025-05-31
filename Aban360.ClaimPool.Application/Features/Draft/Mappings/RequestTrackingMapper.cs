using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestTrackingMapper : Profile
    {
        public RequestTrackingMapper()
        {
            CreateMap<RequestTrackingCreateDto, RequestTracking>();
            CreateMap<RequestTrackingDeleteDto, RequestTracking>();
            CreateMap< RequestTracking, RequestTrackingGetDto>();
        }
    }
}
