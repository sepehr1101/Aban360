using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.AspNetCore.Http;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    public sealed class UserLoginAddHandler : IUserLoginAddHandler
    {
        private readonly IUserLoginCommandService _userLoginService;
        private readonly IHttpContextAccessor _httpContext;
        public UserLoginAddHandler(
            IUserLoginCommandService userLoginService,
            IHttpContextAccessor httpContext)
        {
            _userLoginService = userLoginService;
            _userLoginService.NotNull(nameof(userLoginService));

            _httpContext = httpContext;
            _httpContext.NotNull(nameof(httpContext));
        }
        public async Task<FirstStepOutput> Handle(FirstStepLoginInput input, User user)
        {
            Random rand = new Random();
            UserLogin userLogin = new UserLogin()
            {
                Id = Guid.NewGuid(),
                AppVersion = input.AppVersion,
                FirstStepDateTime = DateTime.Now,
                Username = input.Username,
                UserId = user.Id,
                LogInfo = LogInfoJson.Get(_httpContext.HttpContext.Request, true),
                TwoStepCode = rand.Next(1000, 9999).ToString(),
                TwoStepExpireDateTime = DateTime.Now.AddMinutes(120),
                PreviousFailureIsShown = false,
                Ip = _httpContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            };
            await _userLoginService.Add(userLogin);
            return new FirstStepOutput(userLogin.Id, 120, userLogin.TwoStepCode);
        }
    }
}
