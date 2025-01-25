using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserLoginFindHandler
    {
        Task<UserLogin> Handle(SecondStepLoginInput input, CancellationToken cancellationToken);
    }
}