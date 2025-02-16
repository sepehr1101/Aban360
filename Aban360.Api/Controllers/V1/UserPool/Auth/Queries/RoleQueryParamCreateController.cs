using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/role")]
    public class RoleQueryParamCreateController : ControllerBase
    {
        private readonly IRoleQueryParamsOfCreateHandler _roleQueryParamsOfCreateHandler;
        public RoleQueryParamCreateController(IRoleQueryParamsOfCreateHandler roleQueryParamsOfCreateHandler)
        {
            _roleQueryParamsOfCreateHandler = roleQueryParamsOfCreateHandler;
            _roleQueryParamsOfCreateHandler.NotNull(nameof(_roleQueryParamsOfCreateHandler));
        }

        [HttpGet,HttpPost]
        [Route("params-create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleParamsOfCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoleParamsOfCreate(CancellationToken cancellationToken)
        {
            RoleParamsOfCreateDto roleParamsOfCreateDto= await _roleQueryParamsOfCreateHandler.Handle(cancellationToken); 
            return Ok(roleParamsOfCreateDto);
        }
    }
}
