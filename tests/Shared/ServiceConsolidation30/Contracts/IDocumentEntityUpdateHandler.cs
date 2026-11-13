// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Update.Contracts
{
    public interface IDocumentEntityUpdateHandler
    {
        Task Handle(DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
