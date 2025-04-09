using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts
{
    public interface IDocumentCommandService
    {
        Task Add(Document document);
        Task Remove(Document document);
    }
}
