using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IRequestBillDetailQueryService
    {
        Task<RequestBillDetailGetDto> Get(int id, string billId);
    }
}
