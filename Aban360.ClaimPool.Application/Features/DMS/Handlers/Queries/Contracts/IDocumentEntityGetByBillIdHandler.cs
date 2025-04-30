using Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts
{
    public interface IDocumentEntityGetByBillIdHandler
    {
        Task<ICollection<DocumentEntityGetDto>> Handle(string billId, CancellationToken cancellationToken);
    }
}
