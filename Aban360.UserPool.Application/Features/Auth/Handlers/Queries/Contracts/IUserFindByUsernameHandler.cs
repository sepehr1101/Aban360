using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserFindByUsernameHandler
    {
        Task<(User, bool)> Handle(string username, string plainPassword);
    }
}