using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/role")]
    public class RoleGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleGetSingleHandler _roleGetSingleHandler;
        public RoleGetSingleController(
            IUnitOfWork uow,
            IRoleGetSingleHandler roleGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _roleGetSingleHandler = roleGetSingleHandler;
            _roleGetSingleHandler.NotNull(nameof(roleGetSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var role = await _roleGetSingleHandler.Handle(id, cancellationToken);
            return Ok(role);
        }
    }
}
