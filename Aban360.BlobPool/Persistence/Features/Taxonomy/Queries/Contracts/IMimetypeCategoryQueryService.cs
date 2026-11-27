using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    public interface IMimetypeCategoryQueryService
    {
        Task<ICollection<MimetypeCategory>> Get();
        Task<MimetypeCategory> Get(short id);
    }
}