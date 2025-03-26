using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v2/usage-level-2")]
    public class UsageLevel2GetSingleController : BaseController
    {
        private readonly IUsageLevel2GetSingleHandler _usageLevel2GetSingleHandler;
        public UsageLevel2GetSingleController(IUsageLevel2GetSingleHandler UsageLevel2GetSingleHandler)
        {
            _usageLevel2GetSingleHandler = UsageLevel2GetSingleHandler;
            _usageLevel2GetSingleHandler.NotNull(nameof(UsageLevel2GetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel2GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel2s = await _usageLevel2GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel2s);
        }
    }
}
