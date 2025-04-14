using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
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
