using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Request.Mappings
{
    public class GatewayMapper : Profile
    {
        public GatewayMapper()
        {
            CreateMap<GatewayCreateDto, Gateway>();
            CreateMap<GatewayDeleteDto, Gateway>();
            CreateMap<GetewayUpdateDto, Gateway>();
            CreateMap<Gateway,GatewayGetDto>();
        }
    }
}
