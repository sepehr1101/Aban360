using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IFindUserByUsernamePasswordHandler
    {       
        Task<(User?, bool)> Handle(FirstStepLoginInput input, CancellationToken cancellationToken);
    }
}