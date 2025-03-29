using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/usage-level-1")]
    public class UsageLevel1GetSingleController : BaseController
    {
        private readonly IUsageLevel1GetSingleHandler _usageLevel1GetSingleHandler;
        public UsageLevel1GetSingleController(IUsageLevel1GetSingleHandler UsageLevel1GetSingleHandler)
        {
            _usageLevel1GetSingleHandler = UsageLevel1GetSingleHandler;
            _usageLevel1GetSingleHandler.NotNull(nameof(UsageLevel1GetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel1s = await _usageLevel1GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel1s);
        }
    }
}
