using Aban360.BlobPool.Domain.Features.Taxonomy;

namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts
{
    internal interface IExecutableMimetypeQueryService
    {
        Task<ICollection<ExecutableMimetype>> Get();
        Task<ExecutableMimetype> Get(int id);
    }
}