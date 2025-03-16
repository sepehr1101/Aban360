using Aban360.ClaimPool.Domain.Features.Draff.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draff.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draff.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draff.Mappings
{
    internal class RequestUserMapper : Profile
    {
        public RequestUserMapper()
        {
            CreateMap< RequestUserCommandDto,  RequestUser>();
            CreateMap< RequestUser,  RequestUserQueryDto>();
        }
    }
}
