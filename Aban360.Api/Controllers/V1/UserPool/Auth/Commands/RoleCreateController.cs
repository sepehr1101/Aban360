using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/role")]
    public class RoleCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleCreateHandler _roleCreateHandler;
        public RoleCreateController(
            IUnitOfWork uow, 
            IRoleCreateHandler roleCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));  

            _roleCreateHandler = roleCreateHandler;
            _roleCreateHandler.NotNull(nameof(roleCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _roleCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
