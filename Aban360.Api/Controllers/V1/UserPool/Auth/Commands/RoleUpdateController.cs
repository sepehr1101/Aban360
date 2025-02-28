using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/role")]
    public class RoleUpdateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleUpdateHandler _roleUpdateHandler;
        public RoleUpdateController(
            IUnitOfWork uow,
            IRoleUpdateHandler roleUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));  

            _roleUpdateHandler = roleUpdateHandler;
            _roleUpdateHandler.NotNull(nameof(roleUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _roleUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
