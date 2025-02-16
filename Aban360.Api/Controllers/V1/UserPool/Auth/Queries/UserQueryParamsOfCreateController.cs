using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class UserQueryParamsOfCreateController : BaseController
    {
        private readonly IUserQueryParamsOfCreate _userQueryParamsOfCreate;
        public UserQueryParamsOfCreateController(IUserQueryParamsOfCreate userQueryParamsOfCreate)
        {
            _userQueryParamsOfCreate = userQueryParamsOfCreate;
            _userQueryParamsOfCreate.NotNull(nameof(userQueryParamsOfCreate));
        }
                
        [HttpGet,HttpPost]
        [Route("params-create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserParamsOfCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParamsOfCreate(CancellationToken cancellationToken)
        {
            UserParamsOfCreateDto userParamsOfCreateDto= await _userQueryParamsOfCreate.Handle(cancellationToken);
            return Ok(userParamsOfCreateDto);
        }
    }
}
