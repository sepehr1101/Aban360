using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/usage-level-4")]
    public class UsageLevel4GetAllController : BaseController
    {
        private readonly IUsageLevel4GetAllHandler _usageLevel4GetAllHandler;
        public UsageLevel4GetAllController(IUsageLevel4GetAllHandler usageLevel4GetAllHandler)
        {
            _usageLevel4GetAllHandler = usageLevel4GetAllHandler;
            _usageLevel4GetAllHandler.NotNull(nameof(usageLevel4GetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageLevel4GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var usageLevel4s = await _usageLevel4GetAllHandler.Handle(cancellationToken);
            return Ok(usageLevel4s);
        }
    }
}
