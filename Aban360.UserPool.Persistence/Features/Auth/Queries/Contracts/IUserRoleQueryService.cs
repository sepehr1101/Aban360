using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IUserRoleQueryService
    {
        Task<ICollection<UserRole>> Get(Guid userId);
        Task<ICollection<UserRole>> GetIncludeRoles(Guid userId);
        Task<ICollection<UserRole>> Get(string roleTitle);
        Task<ICollection<UserQueryDto>> Get(int roleId);
    }
}