using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IUserRoleQueryService
    {
        Task<ICollection<UserRole>> Get(Guid userId);
        Task<ICollection<UserRole>> GetIncludeRoles(Guid userId);
    }
}