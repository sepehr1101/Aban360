using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v3/usage-level-3")]
    public class UsageLevel3GetSingleController : BaseController
    {
        private readonly IUsageLevel3GetSingleHandler _usageLevel3GetSingleHandler;
        public UsageLevel3GetSingleController(IUsageLevel3GetSingleHandler UsageLevel3GetSingleHandler)
        {
            _usageLevel3GetSingleHandler = UsageLevel3GetSingleHandler;
            _usageLevel3GetSingleHandler.NotNull(nameof(UsageLevel3GetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel3GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel3s = await _usageLevel3GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel3s);
        }
    }
}
