using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IDocumentGetByBillIdCategoryIdHandler
    {
        Task<ICollection<DocumentGetDto>> Handle(short documentCategoryId, string billId, CancellationToken cancellationToken);    }
}
