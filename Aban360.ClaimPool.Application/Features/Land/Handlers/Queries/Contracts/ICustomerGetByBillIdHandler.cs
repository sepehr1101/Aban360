using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface ICustomerGetByBillIdHandler
    {
        Task<SubscriptionGetDto> Handle(SearchInput inputDto, CancellationToken cancellationToken);
    }
}
