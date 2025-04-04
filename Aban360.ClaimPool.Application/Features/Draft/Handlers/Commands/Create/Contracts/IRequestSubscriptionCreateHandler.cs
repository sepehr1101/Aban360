using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestSubscriptionCreateHandler
    {
        Task Handle(RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken);
    }
}
