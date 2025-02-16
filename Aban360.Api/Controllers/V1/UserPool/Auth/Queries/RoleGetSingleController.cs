using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/role")]
    public class RoleGetSingleController : BaseController
    {      
        private readonly IRoleGetSingleHandler _roleGetSingleHandler;
        private readonly IAccessTreeValueKeyQueryHandler _accessTreeValueKeyQueryHandler;

        public RoleGetSingleController(
            IRoleGetSingleHandler roleGetSingleHandler,
            IAccessTreeValueKeyQueryHandler accessTreeValueKeyQueryHandler)
        {            
            _roleGetSingleHandler = roleGetSingleHandler;
            _roleGetSingleHandler.NotNull(nameof(roleGetSingleHandler));

            _accessTreeValueKeyQueryHandler = accessTreeValueKeyQueryHandler;
            _accessTreeValueKeyQueryHandler.NotNull(nameof(accessTreeValueKeyQueryHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleDisplayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            RoleDisplayDto roleDisplayDto = await _roleGetSingleHandler.Handle(id, cancellationToken);
            return Ok(roleDisplayDto);
        }
    }
}
