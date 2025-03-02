using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IProfessionQueryService
    {
        Task<Profession> Get(short id);
        Task<ICollection<Profession>> Get();
    }
}
