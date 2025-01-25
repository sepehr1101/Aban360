using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts
{
    public interface IUserLoginCommandService
    {
        Task Add(UserLogin userLogin);
    }
}