using Aban360.ClaimPool.Domain.Features.Draff.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draff.Queries.Contracts
{
    public interface IRequestUserQueryService
    {
        Task<RequestUser> Get(short id);
        Task<ICollection<RequestUser>> Get();
    }
}
