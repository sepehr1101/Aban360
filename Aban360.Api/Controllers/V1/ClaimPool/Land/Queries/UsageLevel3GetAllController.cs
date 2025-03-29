using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v3/usage-level-3")]
    public class UsageLevel3GetAllController : BaseController
    {
        private readonly IUsageLevel3GetAllHandler _usageLevel3GetAllHandler;
        public UsageLevel3GetAllController(IUsageLevel3GetAllHandler usageLevel3GetAllHandler)
        {
            _usageLevel3GetAllHandler = usageLevel3GetAllHandler;
            _usageLevel3GetAllHandler.NotNull(nameof(usageLevel3GetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel3GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel3s = await _usageLevel3GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel3s);
        }
    }
}
