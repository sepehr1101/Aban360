using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/captcha")]
    [ApiController]
    public class CaptchaCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICaptchaCreateHandler _captchaCreateHandler;
        public CaptchaCreateController(
            IUnitOfWork uow,
            ICaptchaCreateHandler captchaCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _captchaCreateHandler = captchaCreateHandler;
            _captchaCreateHandler.NotNull(nameof(captchaCreateHandler));
        }

        [Route("create")]
        [HttpPost, HttpPatch]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CaptchaCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CaptchaCreateDto capthcaCreateDto, CancellationToken cancellationToken)
        {
           await _captchaCreateHandler.Handle(capthcaCreateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(capthcaCreateDto);
        }
    }
}