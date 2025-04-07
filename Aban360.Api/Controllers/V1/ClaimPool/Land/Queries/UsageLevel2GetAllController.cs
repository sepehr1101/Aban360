using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/usage-level-2")]
    public class UsageLevel2GetAllController : BaseController
    {
        private readonly IUsageLevel2GetAllHandler _usageLevel2GetAllHandler;
        public UsageLevel2GetAllController(IUsageLevel2GetAllHandler usageLevel2GetAllHandler)
        {
            _usageLevel2GetAllHandler = usageLevel2GetAllHandler;
            _usageLevel2GetAllHandler.NotNull(nameof(usageLevel2GetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel2GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel2s = await _usageLevel2GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel2s);
        }
    }
}
