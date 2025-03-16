using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Request.Mappings
{
    public class GetewayMapper : Profile
    {
        public GetewayMapper()
        {
            CreateMap<GetewayCreateDto, Geteway>();
            CreateMap<GetewayDeleteDto, Geteway>();
            CreateMap<GetewayUpdateDto, Geteway>();
            CreateMap<Geteway,GetewayGetDto>();
        }
    }
}
