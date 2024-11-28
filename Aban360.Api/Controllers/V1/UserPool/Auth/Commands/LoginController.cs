﻿using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Application.Features.Auth.Services;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Application.Features.Auth.Services.Contracts;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("login")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly IDNTCaptchaApiProvider _captchaApiProvider;
        private readonly IDNTCaptchaValidatorService _captchaValidatorService;
        private readonly DNTCaptchaOptions _captchaOptions;
        private readonly ICaptchaGetSingleHandler _captchaGetSingleHandler;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly IUserFindByUsernameHandler _userFindByUsernameHandler;

        public LoginController(
            IDNTCaptchaApiProvider captchaApiProvider,
            IDNTCaptchaValidatorService captchaValidatorService,
            IOptions<DNTCaptchaOptions> captchaOptions,
            ICaptchaGetSingleHandler captchaGetSingleHandler,
            ITokenFactoryService tokenFactoryService,
            IUserFindByUsernameHandler userFindByUsernameHandler)
        {
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
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("first-step")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PaceFirstStep([FromForm] FirstStepLoginInput loginInput)
        {
            //if (!_captchaValidatorService.HasRequestValidCaptchaEntry())
            //{
                //return ClientError(MessageResources.CaptchaInvalid);
            //}
            var (user, result) = await _userFindByUsernameHandler.Handle(loginInput.Username, loginInput.Password);
            var jwtData= await _tokenFactoryService.CreateJwtTokensAsync(user);
            //var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
            //await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
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
    }
}