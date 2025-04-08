using Aban360.BlobPool.Domain.Features.Taxonomy;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    internal interface IMimetypeCategoryQueryService
    {
        Task<ICollection<MimetypeCategory>> Get();
        Task<MimetypeCategory> Get(int id);
    }
}