using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestUserQueryService
    {
        Task<RequestUser> Get(short id);
        Task<ICollection<RequestUser>> Get();
    }
}
