using Aban360.ClaimPool.Domain.Features.DMS.Entities;

namespace Aban360.ClaimPool.Persistence.Features.DMS.Commands.Contracts
{
    public interface IDocumentEntityCommandService
    {
        Task Add(DocumentEntity documentEntity);
        Task Remove(DocumentEntity documentEntity);
    }
}
