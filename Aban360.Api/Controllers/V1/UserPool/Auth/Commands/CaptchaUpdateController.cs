using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("captcha")]
    [ApiController]
    public class CaptchaUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICaptchaUpdateHandler _captchaUpdateHandler;
        public CaptchaUpdateController(
            IUnitOfWork uow,
            ICaptchaUpdateHandler captchaUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _captchaUpdateHandler = captchaUpdateHandler;
            _captchaUpdateHandler.NotNull(nameof(captchaUpdateHandler));
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CaptchaUpdateDto capthcaUpdateDto, CancellationToken cancellationToken)
        {
            _captchaUpdateHandler.Handle(capthcaUpdateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(capthcaUpdateDto);
        }
    }
}