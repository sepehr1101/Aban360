using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IHandoverQueryService
    {
        Task<Handover> Get(short id);
        Task<ICollection<Handover>> Get();
    }
}
