using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Implementations
{
    public sealed class UserLoginAddHandler : IUserLoginAddHandler
    {
        private readonly IUserLoginCommandService _userLoginService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IValidator<FirstStepLoginInput> _roleValidator;
        public UserLoginAddHandler(
            IUserLoginCommandService userLoginService,
            IHttpContextAccessor httpContext,
            IValidator<FirstStepLoginInput> roleValidator)
        {
            _userLoginService = userLoginService;
            _userLoginService.NotNull(nameof(userLoginService));

            _httpContext = httpContext;
            _httpContext.NotNull(nameof(httpContext));

            _roleValidator = roleValidator;
            _roleValidator.NotNull(nameof(roleValidator));
        }
        public async Task<FirstStepOutput> Handle(FirstStepLoginInput input, User user)
        {
            var validationResult = await _roleValidator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }//


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
