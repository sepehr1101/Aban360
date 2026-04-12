using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IPreviousRequestGetByBillIdHandler
    {
        Task<IEnumerable<PreviousRequestDataOutputDto>> Handle(string billId, CancellationToken cancellationToken);
    }
}
