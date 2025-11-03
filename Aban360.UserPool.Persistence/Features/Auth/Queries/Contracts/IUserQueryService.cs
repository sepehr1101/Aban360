using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using static Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations.UserQueryService;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface IUserQueryService
    {
        IQueryable<User> GetQuery();
        Task<ICollection<User>> Get();
        Task<User> Get(Guid id);
        Task<User?> Get(string username);
        Task<User> GetIncludeUserAndClaims(Guid userId);
        Task<IEnumerable<UserQueryDto>> Search(SearchUserDto input);
    }
}