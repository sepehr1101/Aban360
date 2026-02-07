using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface ICustomerUpdateHandler
    {
        Task Handle(SubscriptionGetDto inputDto, CancellationToken cancellationToken);
        Task Handle(CustomerUpdate1Dto inputDto, CancellationToken cancellationToken);
        Task Handle(CustomerUpdate2Dto inputDto, CancellationToken cancellationToken);
        Task Handle(CustomerUpdate3Dto inputDto, CancellationToken cancellationToken);
        Task Handle(CustomerUpdate5Dto inputDto, CancellationToken cancellationToken);
    }
}
