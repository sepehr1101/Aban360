using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations;

// برای اجرای Jobهای قبلاً ذخیره‌شده در Hangfire، قرارداد قبلی حفظ شده است.
// منطق عملیات فقط در سرویس تجمیع‌شده قرار دارد.
internal sealed class UpdateUsersInRoleHandler : IUpdateUsersInRoleHandler
{
    private readonly IUserRoleMembershipService _service;

    public UpdateUsersInRoleHandler(IUserRoleMembershipService service)
    {
        service.NotNull(nameof(service));
        _service = service;
    }

    public Task Handle(int roleId, CancellationToken cancellationToken) =>
        _service.UpdateUsersInRoleAsync(roleId, cancellationToken);
}
