// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IExecutableMimetypeGetAllHandler
    {
        Task<ICollection<ExecutableMimetypeGetDto>> Handle(CancellationToken cancellationToken);
    }
}
