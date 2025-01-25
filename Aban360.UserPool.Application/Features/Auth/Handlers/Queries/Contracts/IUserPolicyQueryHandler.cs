using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserPolicyQueryHandler
    {
        Task<(string, bool)> Handle(FirstStepLoginInput input, CancellationToken cancellationToken);
    }
}