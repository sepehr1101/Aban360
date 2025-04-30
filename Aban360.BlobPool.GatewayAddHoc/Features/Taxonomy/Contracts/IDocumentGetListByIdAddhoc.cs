using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts
{
    public interface IDocumentGetListByIdAddhoc
    {
        Task<ICollection<Document>> Handle(ICollection<Guid> ids, CancellationToken cancellationToken);
    }
}
