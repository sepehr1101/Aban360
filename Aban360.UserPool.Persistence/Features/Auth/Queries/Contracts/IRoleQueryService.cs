using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IRoleQueryService
    {
        Task<ICollection<Role>> Get();
        Task<Role> Get(int id);
    }
}