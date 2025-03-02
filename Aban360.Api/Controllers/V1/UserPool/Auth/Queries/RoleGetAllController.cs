using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/role")]
    public class RoleGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleGetAllHandler _roleGetAllHandler;
        public RoleGetAllController(
            IUnitOfWork uow,
            IRoleGetAllHandler roleGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _roleGetAllHandler = roleGetAllHandler;
            _roleGetAllHandler.NotNull(nameof(roleGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RoleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var role = await _roleGetAllHandler.Handle(cancellationToken);
            return Ok(role);
        }
    }
}
