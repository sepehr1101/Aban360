using Aban360.ClaimPool.Domain.Features.DMS.Entities;

namespace Aban360.ClaimPool.Persistence.Features.DMS.Queries.Contracts
{
    public interface IDocumentEntityQueryService
    {
        Task<DocumentEntity> Get(long id);
        Task<ICollection<DocumentEntity>> Get(string id);
        Task<ICollection<DocumentEntity>> Get();
    }
}
