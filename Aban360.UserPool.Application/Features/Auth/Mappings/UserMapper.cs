using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class UserMapper:Profile
    {
        public UserMapper()
        {
            CreateMap<UserCreateDto, User>();
        }
    }
}
