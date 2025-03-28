using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    internal class RequestUserMapper : Profile
    {
        public RequestUserMapper()
        {
            CreateMap<RequestUserCommandDto, RequestUser>();
            CreateMap<RequestUser, RequestUserQueryDto>();
        }
    }
}
