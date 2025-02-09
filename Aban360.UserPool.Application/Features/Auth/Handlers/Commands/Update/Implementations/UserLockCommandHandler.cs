using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public class UserLockCommandHandler : IUserLockCommandHandler
    {
        private readonly IUserQueryService _userQueryService;
        public UserLockCommandHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(UserIdDto userId, CancellationToken cancellationToken)
        {
            var user = await _userQueryService.Get(userId.Id);
            if (user == null)
            {
                throw new InvalidDataException();
            }

            user.LockTimespan = DateTime.Now;
        }
    }
}
