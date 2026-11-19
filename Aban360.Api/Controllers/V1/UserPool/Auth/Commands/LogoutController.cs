using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/session")]
    public class LogoutController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserTokenService _userTokenService;
        public LogoutController(IUnitOfWork uow, IUserTokenService userTokenService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _userTokenService = userTokenService;
            _userTokenService.NotNull(nameof(_userTokenService));
        }

        [Route("terminate")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> TerminateSession([FromBody] RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            UserToken userToken = await _userTokenService.FindByRefreshAsync(refreshToken, cancellationToken);
            if (userToken == null)
            {
                return Unauthorized();
            }

            await _userTokenService.DeleteAsync(userToken.UserId, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok("خروج با موفقت انجام شد");
        }
    }
}
