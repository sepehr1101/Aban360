using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    public interface IExecutableMimetypeQueryService
    {
        Task<ICollection<ExecutableMimetype>> Get();
        Task<ExecutableMimetype> Get(short id);
    }
}