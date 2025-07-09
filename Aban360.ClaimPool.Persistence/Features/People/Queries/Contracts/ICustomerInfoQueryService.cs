using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts
{
    public interface ICustomerInfoQueryService
    {
        Task<CustomerInfoGetDto> Get(string billId);
    }
}
