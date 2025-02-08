using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/role")]
    public class RoleDeleteController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleDeleteHandler _roleDeleteHandler;
        public RoleDeleteController(
            IUnitOfWork uow,
            IRoleDeleteHandler roleDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));  

            _roleDeleteHandler = roleDeleteHandler;
            _roleDeleteHandler.NotNull(nameof(roleDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RoleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _roleDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
