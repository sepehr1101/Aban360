using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Common.Constants;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Base;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IUserPasswordService
{
    Task ChangePasswordAsync(ChangePasswordInput changePasswordInput, Guid userId, CancellationToken cancellationToken);
    Task ResetPasswordAsync(UserIdDto userId, CancellationToken cancellationToken);
}

internal sealed class UserPasswordService : IUserPasswordService
{
    private readonly IUserQueryService _userQueryService;
    public UserPasswordService(IUserQueryService userQueryService)
    {
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
    }

    public async Task ChangePasswordAsync(ChangePasswordInput changePasswordInput, Guid userId, CancellationToken cancellationToken)
    {
        ChangePasswordValidate(changePasswordInput);
        User user = await _userQueryService.Get(userId);
        user.Password = await SecurityOperations.GetSha512Hash(changePasswordInput.Password);
    }

    private void ChangePasswordValidate(ChangePasswordInput changePasswordInput)
    {
        if (changePasswordInput.Password != changePasswordInput.PasswordConfirm)
        {
            new ChangePasswordValidationException(MessageLiterals.PasswordAndConfirmNotMatch);
        }
    }

    public async Task ResetPasswordAsync(UserIdDto userId, CancellationToken cancellationToken)
    {
        User user = await _userQueryService.Get(userId.Id);
        user.Password = await SecurityOperations.GetSha512Hash(user.Mobile);
    }
}
