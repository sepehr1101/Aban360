using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
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
