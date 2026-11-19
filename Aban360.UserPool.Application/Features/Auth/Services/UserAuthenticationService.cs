using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading;

namespace Aban360.UserPool.Application.Features.Auth.Services;
public interface IUserAuthenticationService
{
    Task<UserLogin> FindLoginAsync(SecondStepLoginInput input, CancellationToken cancellationToken);
    Task<FirstStepOutput> AddLoginAsync(FirstStepLoginInput input, User user);
    Task LogoutAsync(Guid userId, LogoutReasonEnum logoutReasonId);
    Task<(User, bool)> FindByUsernameAsync(string username, string plainPassword);
    Task<(User? , bool)> FindByCredentialsAsync(FirstStepLoginInput input, CancellationToken cancellationToken);
}

internal sealed class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IUserLoginQueryService _userLoginQueryService;
    private readonly IUserLoginCommandService _userLoginService;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IValidator<FirstStepLoginInput> _roleValidator;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userQueryService;
    public UserAuthenticationService(IUserLoginQueryService userLoginQueryService, IUserLoginCommandService userLoginService, IHttpContextAccessor httpContext, IValidator<FirstStepLoginInput> roleValidator, IMapper mapper, IUserQueryService userQueryService)
    {
        _userLoginQueryService = userLoginQueryService;
        _userLoginQueryService.NotNull(nameof(userLoginQueryService));
        _userLoginService = userLoginService;
        _userLoginService.NotNull(nameof(userLoginService));
        _httpContext = httpContext;
        _httpContext.NotNull(nameof(httpContext));
        _roleValidator = roleValidator;
        _roleValidator.NotNull(nameof(roleValidator));
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _userQueryService = userQueryService;
        _userQueryService.NotNull(nameof(userQueryService));
        _userQueryService.NotNull(nameof(_userQueryService));
        _userLoginService.NotNull(nameof(_userLoginService));
        _httpContext.NotNull(nameof(_httpContext));
    }

    public async Task<UserLogin> FindLoginAsync(SecondStepLoginInput input, CancellationToken cancellationToken)
    {
        UserLogin userLogin = await _userLoginQueryService.Get(input.Id);
        userLogin.NotNull(nameof(userLogin));
        userLogin.TwoStepCode.NotEmptyString(userLogin.TwoStepCode);
        if (userLogin.TwoStepCode != input.ConfirmCode)
        {
            throw new Exception("کد وارد شده صحیح نمیباشد");
        }

        return userLogin;
    }

    public async Task<FirstStepOutput> AddLoginAsync(FirstStepLoginInput input, User user)
    {
        var validationResult = await _roleValidator.ValidateAsync(input);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        } //

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

    public async Task LogoutAsync(Guid userId, LogoutReasonEnum logoutReasonId)
    {
        ICollection<UserLogin> userLogins = await _userLoginQueryService.GetByUserId(userId);
        if (userLogins is null || !userLogins.Any())
        {
            return;
        }

        userLogins.ForEach(l =>
        {
            l.LogoutDateTime = DateTime.Now;
            l.LogoutReasonId = logoutReasonId;
        });
    }

    public async Task<(User, bool)> FindByUsernameAsync(string username, string plainPassword)
    {
        User? user = await _userQueryService.Get(username);
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        string password = await SecurityOperations.GetSha512Hash(plainPassword);
        if (user.Password != password)
        {
            throw new ArgumentException(nameof(password));
        }

        return (user, true);
    }

    public async Task<(User? , bool)> FindByCredentialsAsync(FirstStepLoginInput input, CancellationToken cancellationToken)
    {
        User user = await _userQueryService.Get(input.Username);
        string logInfoString = FindByCredentialsGetLogInfo();
        if (user == null)
        {
            await FindByCredentialsGetUserLogin(InvalidLoginReasonEnum.InvalidUsername, false, false, input, user);
            return (user, false);
        }
        else
        {
            string hashedPassword = await SecurityOperations.GetSha512Hash(input.Password);
            if (hashedPassword != user.Password)
            {
                await FindByCredentialsGetUserLogin(InvalidLoginReasonEnum.InvalidPassword, true, true, input, user);
                return (user, false);
            }

            if (user.RemoveLogInfo != null && string.IsNullOrWhiteSpace(user.RemoveLogInfo))
            {
                return (user, false);
            }
            else
            {
                return (user, true);
            }
        }
    }

    private string FindByCredentialsGetLogInfo()
    {
        LogInfo logInfo = DeviceDetection.GetLogInfo(_httpContext.HttpContext.Request);
        string logInfoString = JsonOperation.Marshal(logInfo);
        return logInfoString;
    }

    private async Task FindByCredentialsGetUserLogin(InvalidLoginReasonEnum LoginReasonEnum, bool IsUserName, bool IsPassword, FirstStepLoginInput input, User? user)
    {
        UserLogin userLogin = new UserLogin()
        {
            Id = new Guid(),
            Username = IsUserName ? input.Username : null,
            WrongPassword = IsPassword ? input.Password : null,
            UserId = user != null ? user.Id : null,
            FirstStepSuccess = false,
            AppVersion = input.AppVersion,
            FirstStepDateTime = DateTime.Now,
            LogInfo = FindByCredentialsGetLogInfo(),
            Ip = _httpContext.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            InvalidLoginReasonId = LoginReasonEnum,
        };
        await _userLoginService.Add(userLogin);
    }
}
