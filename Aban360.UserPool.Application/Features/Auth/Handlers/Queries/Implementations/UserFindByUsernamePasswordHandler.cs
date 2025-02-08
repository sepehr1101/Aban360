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
    public sealed class UserFindByUsernamePasswordHandler : IUserFindByUsernamePasswordHandler
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IUserLoginCommandService _userLoginCommandService;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserFindByUsernamePasswordHandler(
            IUserQueryService userQueryService,
            IUserLoginCommandService userLoginCommandService,
            IHttpContextAccessor contextAccessor)
        {
            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(_userQueryService));

            _userLoginCommandService = userLoginCommandService;
            _userLoginCommandService.NotNull(nameof(_userLoginCommandService));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(_contextAccessor));
        }
        public async Task<(User?, bool)> Handle(FirstStepLoginInput input, CancellationToken cancellationToken)
        {
            var user = await _userQueryService.Get(input.Username);

            var logInfoString = GetLogInfo();

            if (user == null)
            {
                await GetUserLogin(InvalidLoginReasonEnum.InvalidUsername, false, false, input, user);
                return (user, false);

            }
            else
            {
                var hashedPassword = await SecurityOperations.GetSha512Hash(input.Password);
                if (hashedPassword != user.Password)
                {
                    await GetUserLogin(InvalidLoginReasonEnum.InvalidPassword, true, true, input, user);
                    return (user, false);
                }
                else
                {
                    return (user, true);
                }
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
