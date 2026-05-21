using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/kartable-access")]
    public class KartableAccessGetController : BaseController
    {
        private readonly IRequestKartableGetListByUserIdHandler _requestKartableGetListByUserIdHandler;
        public KartableAccessGetController(IRequestKartableGetListByUserIdHandler requestKartableGetListByUserIdHandler)
        {
            _requestKartableGetListByUserIdHandler = requestKartableGetListByUserIdHandler;
            _requestKartableGetListByUserIdHandler.NotNull(nameof(requestKartableGetListByUserIdHandler));
        }

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SelectionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<SelectionDto> result = await _requestKartableGetListByUserIdHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
