using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts
{
    public interface ICustomerInfoGetByBillIdHandler
    {
        Task<CustomerInfoGetDto > Handle(SearchInput input,CancellationToken cancellationToken);
    }
}
