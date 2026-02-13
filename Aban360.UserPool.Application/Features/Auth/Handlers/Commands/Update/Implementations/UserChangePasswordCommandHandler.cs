using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Constants;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    internal sealed class UserChangePasswordCommandHandler: IUserChangePasswordCommandHandler
    {
        private readonly IUserQueryService _userQueryService;
        public UserChangePasswordCommandHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(ChangePasswordInput changePasswordInput, Guid userId, CancellationToken cancellationToken)
        {
            Validate(changePasswordInput);
            User user = await _userQueryService.Get(userId);
            user.Password = await SecurityOperations.GetSha512Hash(changePasswordInput.Password);            
        }
        private void Validate(ChangePasswordInput changePasswordInput)
        {
            if (changePasswordInput.Password != changePasswordInput.PasswordConfirm)
            {
                new ChangePasswordValidationException(MessageLiterals.PasswordAndConfirmNotMatch);
            }
        }
    }
}
