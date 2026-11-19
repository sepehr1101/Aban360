using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IUserLockService
{
    Task LockAsync(UserIdDto userId, CancellationToken cancellationToken);
    Task UnLockAsync(UserIdDto userId, CancellationToken cancellationToken);
}

internal sealed class UserLockService : IUserLockService
{
    private readonly IUserQueryService _userQueryService;
    public UserLockService(IUserQueryService userQueryService)
    {
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
    }

    public async Task LockAsync(UserIdDto userId, CancellationToken cancellationToken)
    {
        User user = await _userQueryService.Get(userId.Id);
        user.LockTimespan = DateTime.Now;
    }

    public async Task UnLockAsync(UserIdDto userId, CancellationToken cancellationToken)
    {
        User user = await _userQueryService.Get(userId.Id);
        user.LockTimespan = null;
    }
}
