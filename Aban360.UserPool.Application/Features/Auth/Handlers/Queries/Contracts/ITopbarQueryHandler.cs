using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ITopbarQueryHandler
    {
        Task<Topbar> Handle(Guid userId, CancellationToken cancellationToken);
    }
}