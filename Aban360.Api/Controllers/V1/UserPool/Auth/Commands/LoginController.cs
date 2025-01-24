using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("login")]
    public class LoginController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        private readonly IDNTCaptchaValidatorService _captchaValidatorService;
        private readonly DNTCaptchaOptions _captchaOptions;
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IUserFindByUsernameQueryHandler _userFindByUsernameHandler;
        private readonly IUserTokenCreateHandler _userTokenCreateHandler;
        private readonly IUserTokenFindByRefreshQueryHandler _userTokenFindByRefreshTokenHandler;

        public LoginController(
            IUnitOfWork uow,
            IDNTCaptchaApiProvider captchaApiProvider,
            IDNTCaptchaValidatorService captchaValidatorService,
            IOptions<DNTCaptchaOptions> captchaOptions,
            ICaptchaGetSingleHandler captchaGetSingleHandler,
            ITokenFactoryService tokenFactoryService,
            IUserFindByUsernameQueryHandler userFindByUsernameHandler,
            IUserTokenCreateHandler userTokenCreateHandler,
            IUserTokenFindByRefreshQueryHandler userTokenFindByRefreshTokenHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _captchaApiProvider = captchaApiProvider;
            _captchaApiProvider.NotNull(nameof(_captchaApiProvider));

            _captchaValidatorService = captchaValidatorService;
            _captchaValidatorService.NotNull(nameof(_captchaValidatorService));

            _captchaOptions = captchaOptions.Value;
            _captchaOptions.NotNull(nameof(_captchaOptions));

            _captchaGetSingleHandler = captchaGetSingleHandler;
            _captchaGetSingleHandler.NotNull(nameof(_captchaGetSingleHandler));

            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.NotNull(nameof(_tokenFactoryService));

            _userFindByUsernameHandler = userFindByUsernameHandler;
            _userFindByUsernameHandler.NotNull(nameof(_userFindByUsernameHandler));

            _userTokenCreateHandler = userTokenCreateHandler;
            _userTokenCreateHandler.NotNull(nameof(_userTokenCreateHandler));

            _userTokenFindByRefreshTokenHandler = userTokenFindByRefreshTokenHandler;
            _userTokenFindByRefreshTokenHandler.NotNull(nameof(userTokenFindByRefreshTokenHandler));    
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("first-step")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceFirstStep([FromForm] FirstStepLoginInput loginInput, CancellationToken cancellationToken)
        {
            //if (!_captchaValidatorService.HasRequestValidCaptchaEntry())
            //{
                //return ClientError(MessageResources.CaptchaInvalid);
            //}
            var (user, result) = await _userFindByUsernameHandler.Handle(loginInput.Username, loginInput.Password);
            var jwtData = await _tokenFactoryService.CreateJwtTokensAsync(user);
            await _userTokenCreateHandler.Handle(jwtData, null, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(new { access = jwtData.AccessToken, refresh = jwtData.RefreshToken });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("second-step")]
        //[ProducesResponseType(typeof(LoginOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceSecondStep()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        //[ProducesResponseType(typeof(LoginOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            //var refreshTokenValue = refreshTokenDto.RefreshToken;
            //if (string.IsNullOrWhiteSpace(refreshToken))
            //{
            //    return BadRequest("refreshToken is not set.");
            //}
            var userToken = await _userTokenFindByRefreshTokenHandler.Handle(refreshToken, cancellationToken);
            if (userToken == null)
            {
                return Unauthorized();
            }
            var refreshSerial= _tokenFactoryService.GetRefreshTokenSerial(refreshToken.ToString());
            var jwtData = await _tokenFactoryService.CreateJwtTokensAsync(userToken.User);
            await _userTokenCreateHandler.Handle(jwtData, refreshSerial, cancellationToken);            
            await _uow.SaveChangesAsync();
            return Ok(new { access = jwtData.AccessToken, refresh = jwtData.RefreshToken });
        }

    }
}