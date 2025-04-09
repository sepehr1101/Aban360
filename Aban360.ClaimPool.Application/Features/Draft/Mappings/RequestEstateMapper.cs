using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestEstateMapper : Profile
    {
        public RequestEstateMapper()
        {
            CreateMap<EstateRequestCreateDto, RequestEstate>();
            CreateMap<EstateRequestDeleteDto, RequestEstate>();
            CreateMap<EstateRequestUpdateDto, RequestEstate>();
        }
    }
}
