using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/app")]
    public class AppUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAppUpdateHandler _appUpdateHandler;
        public AppUpdateController(
            IUnitOfWork uow,
            IAppUpdateHandler appUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _appUpdateHandler = appUpdateHandler;
            _appUpdateHandler.NotNull(nameof(_appUpdateHandler));
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AppUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] AppUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _appUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
