using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class UserResetPasswordCommandHandler : IUserResetPasswordCommandHandler
    {
        private readonly IUserQueryService _userQueryService;
        public UserResetPasswordCommandHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(UserIdDto userId, CancellationToken cancellationToken)
        {
            User user = await _userQueryService.Get(userId.Id);
            if (user == null)
            {
                throw new InvalidDataException();
            }
            user.Password = await SecurityOperations.GetSha512Hash(user.Mobile);
        }
    }
}
