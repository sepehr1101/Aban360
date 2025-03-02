using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/session")]
    public class LogoutController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserTokenFindByRefreshQueryHandler _userTokenFindByRefreshTokenHandler;
        private readonly IUserTokenDeleteHandler _userTokenDeleteHandler;

        public LogoutController(
            IUnitOfWork uow,
            IUserTokenFindByRefreshQueryHandler userTokenFindByRefreshQueryHandler,
            IUserTokenDeleteHandler userTokenDeleteHandler)
        {
            _uow= uow;
            _uow.NotNull(nameof(uow));

            _userTokenFindByRefreshTokenHandler = userTokenFindByRefreshQueryHandler;
            _userTokenFindByRefreshTokenHandler.NotNull(nameof(_userTokenFindByRefreshTokenHandler));

            _userTokenDeleteHandler = userTokenDeleteHandler;
            _userTokenDeleteHandler.NotNull(nameof(_userTokenDeleteHandler));
        }

        [Route("terminate")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> TerminateSession([FromBody] RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var userToken = await _userTokenFindByRefreshTokenHandler.Handle(refreshToken, cancellationToken);
            if (userToken == null)
            {
                return Unauthorized();
            }
            await _userTokenDeleteHandler.Handle(userToken.UserId, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok("خروج با موفقت انجام شد");
        }
    }
}