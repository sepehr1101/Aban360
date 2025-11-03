using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserSearchHandler
    {
        Task<IEnumerable<UserQueryDto>> Handle(SearchUserDto input, CancellationToken cancellationToken);
    }
}
