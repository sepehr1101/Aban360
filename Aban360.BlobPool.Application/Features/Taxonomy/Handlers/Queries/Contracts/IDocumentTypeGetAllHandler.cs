using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IDocumentTypeGetAllHandler
    {
        Task<ICollection<DocumentTypeGetDto>> Handle(CancellationToken cancellationToken);
    }
}
