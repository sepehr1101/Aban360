using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserSearchByRoleIdHandler
    {
        Task<IEnumerable<UserQueryDto>> Handle(int zoneId, CancellationToken cancellationToken);
    }
}