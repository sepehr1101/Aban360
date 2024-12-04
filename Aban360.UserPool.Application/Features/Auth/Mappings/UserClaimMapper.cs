using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class UserClaimMapper:Profile
    {
        public UserClaimMapper()
        {
            CreateMap<ClaimDto, UserClaim>().ReverseMap();
        }
    }
}
