using Aban360.BlobPool.Domain.Features.DMS.Entities;

namespace Aban360.BlobPool.Persistence.Features.DMS.Commands.Contracts
{
    public interface IDocumentEntityCommandService
    {
        Task Add(DocumentEntity documentEntity);
        Task Remove(DocumentEntity documentEntity);
    }
}
