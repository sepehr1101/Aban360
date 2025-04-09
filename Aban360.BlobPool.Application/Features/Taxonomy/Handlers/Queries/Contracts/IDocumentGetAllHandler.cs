using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IDocumentGetAllHandler
    {
        Task<ICollection<DocumentGetDto>> Handle(CancellationToken cancellationToken);
    }
}
