// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Create.Contracts
{
    public interface IDocumentEntityCreateHandler
    {
        Task Handle(DocumentEntityCreateDto createDto,Guid documentId, CancellationToken cancellationToken);
    }
}
