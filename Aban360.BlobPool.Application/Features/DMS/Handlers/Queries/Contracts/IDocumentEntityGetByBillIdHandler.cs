using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Queries.Contracts
{
    public interface IDocumentEntityGetByBillIdHandler
    {
        Task<ICollection<DocumentEntityGetDto>> Handle(string billId, CancellationToken cancellationToken);
    }
}
