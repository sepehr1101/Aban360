using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.UiElement.Handlers.Queries.Contracts
{
    public interface ITopbarQueryHandler
    {
        Task<Topbar> Handle(Guid userId, CancellationToken cancellationToken);
    }
}