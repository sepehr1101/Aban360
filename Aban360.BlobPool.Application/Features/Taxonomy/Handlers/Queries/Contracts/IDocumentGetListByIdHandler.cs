using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts
{
    public interface IDocumentGetListByIdHandler
    {
        Task<ICollection<Document>> Handle(ICollection<Guid > ids,CancellationToken cancellationToken);
    }
}
