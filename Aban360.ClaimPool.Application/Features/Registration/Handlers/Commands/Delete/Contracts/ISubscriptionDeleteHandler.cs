using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Delete.Contracts
{
    public interface ISubscriptionDeleteHandler
    {
        Task Handle(SubscriptionDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
