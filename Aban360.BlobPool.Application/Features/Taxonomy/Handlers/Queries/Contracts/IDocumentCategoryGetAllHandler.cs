using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IDocumentCategoryGetAllHandler
    {
        Task<ICollection<DocumentCategoryGetDto>> Handle(CancellationToken cancellationToken);
    }
}
