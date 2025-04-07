using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestFlatMapper : Profile
    {
        public RequestFlatMapper()
        {
            CreateMap<FlatRequestCreateDto, RequestFlat>();
            CreateMap<FlatRequestDeleteDto, RequestFlat>();
            CreateMap<FlatRequestUpdateDto, RequestFlat>();
        }
    }
}
