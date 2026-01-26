using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface ICustomerUpdateHandler
    {
        Task Handle(SubscriptionGetDto inputDto, CancellationToken cancellationToken);
    }
}
