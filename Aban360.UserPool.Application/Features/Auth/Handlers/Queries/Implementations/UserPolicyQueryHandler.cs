using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public sealed class UserPolicyQueryHandler : IUserPolicyQueryHandler
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserLoginCommandService _userLoginCommandService;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserPolicyQueryHandler(
            IUserQueryService userQueryService,
             IUserLoginCommandService userLoginCommandService,
            IHttpContextAccessor contextAccessor)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));

            _userLoginCommandService = userLoginCommandService;
            _userLoginCommandService.NotNull(nameof(userLoginCommandService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }

        public async Task<(string, bool)> Handle(FirstStepLoginInput input, CancellationToken cancellationToken)
        {
            var user = await _userQueryService.Get(input.Username);

            if (user.InvalidLoginAttemptCount > 3)//todo: check
            {
                await GetUserLogin(InvalidLoginReasonEnum.LockedUser, true, true, input, user);
                return ("به حداکثر تلاش مجاز رسیده اید", false);
            }
            else if (user.LockTimespan > DateTime.Now)//todo: check
            {
                await GetUserLogin(InvalidLoginReasonEnum.InactiveUser, true, true, input, user);
                var dateLock = user.LockTimespan.Value.Date;
                var timeLock = user.LockTimespan.Value.TimeOfDay;
                return ($"حساب کاربری شما تاریخ {dateLock} - ساعت {timeLock} قفل می باشد ", false);
            }
            else//valid
            {
                //todo: when user validation was true, create userLogin or not??
                return (string.Empty, true);
            }
        }

        private string GetLogInfo()
        {
            var logInfo = DeviceDetection.GetLogInfo(_contextAccessor.HttpContext.Request);
            var logInfoString = JsonOperation.Marshal(logInfo);

            return logInfoString;
        }

        private async Task GetUserLogin(InvalidLoginReasonEnum LoginReasonEnum, bool IsUserName, bool IsPassword, FirstStepLoginInput input,
            User? user)
        {
            var userLogin = new UserLogin()
            {
                Id = new Guid(),
                Username = IsUserName ? input.Username : null,
                WrongPassword = IsPassword ? input.Password : null,
                UserId = user != null ? user.Id : null,
                FirstStepSuccess = false,
                AppVersion = input.AppVersion,
                FirstStepDateTime = DateTime.Now,
                LogInfo = GetLogInfo(),
                Ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                InvalidLoginReasonId = LoginReasonEnum,
            };
            await _userLoginCommandService.Add(userLogin);
        }
    }
}
