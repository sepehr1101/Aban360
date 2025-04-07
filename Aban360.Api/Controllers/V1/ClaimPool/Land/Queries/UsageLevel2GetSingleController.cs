using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/usage-level-2")]
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
