using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using DNTPersianUtils.Core;
using System.Globalization;
using DNTCaptcha.Core;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using System.Diagnostics;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/login")]
    public class LoginController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        private readonly ICaptchaCryptoProvider _captchaCryptoProvider;
        private readonly IDNTCaptchaValidatorService _captchaValidatorService;
        private readonly DNTCaptchaOptions _captchaOptions;
        private readonly ICaptchaService _captchaService;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IUserTokenService _userTokenService;
        private readonly IUserPolicyService _userPolicyQueryHandler;
        private readonly IOpLogCommandService _opLog;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IUnitOfWork uow, IDNTCaptchaApiProvider captchaApiProvider, IDNTCaptchaValidatorService captchaValidatorService, ICaptchaCryptoProvider captchaCryptoProvider, IOptions<DNTCaptchaOptions> captchaOptions, ICaptchaService captchaService, ITokenFactoryService tokenFactoryService, IUserAuthenticationService userAuthenticationService, IUserTokenService userTokenService, IUserPolicyService userPolicyQueryHandler, IOpLogCommandService opLog, ILogger<LoginController> logger)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _captchaApiProvider = captchaApiProvider;
            _captchaApiProvider.NotNull(nameof(_captchaApiProvider));
            _captchaCryptoProvider = captchaCryptoProvider;
            _captchaCryptoProvider.NotNull(nameof(_captchaCryptoProvider));
            _captchaValidatorService = captchaValidatorService;
            _captchaValidatorService.NotNull(nameof(_captchaValidatorService));
            _captchaOptions = captchaOptions.Value;
            _captchaOptions.NotNull(nameof(_captchaOptions));
            _captchaService = captchaService;
            _captchaService.NotNull(nameof(_captchaService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.NotNull(nameof(_tokenFactoryService));
            _userAuthenticationService = userAuthenticationService;
            _userAuthenticationService.NotNull(nameof(_userAuthenticationService));
            _userTokenService = userTokenService;
            _userTokenService.NotNull(nameof(_userTokenService));
            _userTokenService.NotNull(nameof(userTokenService));
            _userPolicyQueryHandler = userPolicyQueryHandler;
            _userPolicyQueryHandler.NotNull(nameof(_userPolicyQueryHandler));
            _opLog = opLog;
            _opLog.NotNull(nameof(opLog));
            _logger = logger;
            _logger.NotNull(nameof(logger));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("first-step")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FirstStepOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SecondStepOutput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceFirstStep([FromBody] FirstStepLoginInput loginInput, CancellationToken cancellationToken)
        {
            bool isCaptchaValid = HasRequestValidCaptchaEntry(loginInput);
            if (!isCaptchaValid && !Debugger.IsAttached)
            {
                return ClientError(MessageResources.CaptchaInvalid);
            }

            var(user, result) = await _userAuthenticationService.FindByCredentialsAsync(loginInput, cancellationToken);
            if (!result || user is null)
            {
                await LogFailedLogin(loginInput.Username, user);
                return ClientError(MessageResources.UserNotFound);
            }

            //Policy
            var(userPolicy, resultPolicy) = await _userPolicyQueryHandler.UserPolicyQueryAsync(loginInput, cancellationToken);
            if (!resultPolicy || !string.IsNullOrEmpty(userPolicy))
            {
                return ClientError(userPolicy);
            }

            if (!user.HasTwoStepVerification)
            {
                SecondStepOutput secondStepOutput = await GetSecondStepOutput(user, cancellationToken);
                return Ok(secondStepOutput);
            }

            FirstStepOutput output = await _userAuthenticationService.AddLoginAsync(loginInput, user);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(output, "second-step");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("second-step")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SecondStepOutput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceSecondStep([FromBody] SecondStepLoginInput loginDto, CancellationToken cancellationToken)
        {
            UserLogin userLogin = await _userAuthenticationService.FindLoginAsync(loginDto, cancellationToken);
            if (userLogin == null)
            {
                return ClientError(MessageResources.InvalidConfirmCode);
            }

            SecondStepOutput secondStepOutput = await GetSecondStepOutput(userLogin.User, cancellationToken);
            return Ok(secondStepOutput);
        }

        private async Task<SecondStepOutput> GetSecondStepOutput(User user, CancellationToken cancellationToken)
        {
            JwtTokenData jwtData = await _tokenFactoryService.CreateJwtTokensAsync(user);
            await _userTokenService.CreateAsync(jwtData, null, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return new SecondStepOutput(jwtData.AccessToken, jwtData.RefreshToken);
        }

        private async Task LogFailedLogin(string? username, User? user)
        {
            const string message = "ورود ناموفق به سامانه؛ نام کاربری یا رمز عبور نادرست است.";
            // Login is anonymous; CurrentUser has no authenticated identity at this point.
            var logUser = new FailedLoginLogUser(user?.Id ?? Guid.Empty, username?.Replace('\r', ' ').Replace('\n', ' ') ?? string.Empty, user?.FullName ?? string.Empty);
            try
            {
                await _opLog.Insert(message, logUser);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to record unsuccessful login in OpLog for username {Username}.", logUser.Username);
            }
        }

        private sealed record FailedLoginLogUser(Guid UserId, string Username, string FullName) : IAppUser
        {
            public ICollection<string> Roles => Array.Empty<string>();
            public bool IsAdmin => false;
        }

        private bool HasRequestValidCaptchaEntry(FirstStepLoginInput input)
        {
            if (string.IsNullOrEmpty(input.CaptchaToken))
            {
                return false;
            }

            if (string.IsNullOrEmpty(input.CaptchaInputText))
            {
                return false;
            }

            input.CaptchaInputText = input.CaptchaInputText.ToEnglishNumbers();
            if (!int.TryParse(input.CaptchaInputText, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var inputNumber))
            {
                return false;
            }

            string decryptedText = _captchaCryptoProvider.Decrypt(input.CaptchaToken);
            string numberToText = inputNumber.ToString(CultureInfo.InvariantCulture);
            if (decryptedText?.Equals(numberToText, StringComparison.Ordinal) != true)
            {
                return false;
            }

            return true;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(SecondStepOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            //var refreshTokenValue = refreshTokenDto.RefreshToken;
            //if (string.IsNullOrWhiteSpace(refreshToken))
            //{
            //    return BadRequest("refreshToken is not set.");
            //}
            UserToken userToken = await _userTokenService.FindByRefreshAsync(refreshToken, cancellationToken);
            if (userToken == null)
            {
                return Unauthorized();
            }

            string refreshSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshToken.ToString());
            JwtTokenData jwtData = await _tokenFactoryService.CreateJwtTokensAsync(userToken.User);
            await _userTokenService.CreateAsync(jwtData, refreshSerial, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok(new SecondStepOutput(jwtData.AccessToken, jwtData.RefreshToken));
        }
    }
}
