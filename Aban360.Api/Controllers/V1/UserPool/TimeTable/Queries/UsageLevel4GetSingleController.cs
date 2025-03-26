using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v4/usage-level-4")]
    public class UsageLevel4GetSingleController : BaseController
    {
        private readonly IUsageLevel4GetSingleHandler _usageLevel4GetSingleHandler;
        public UsageLevel4GetSingleController(IUsageLevel4GetSingleHandler UsageLevel4GetSingleHandler)
        {
            _usageLevel4GetSingleHandler = UsageLevel4GetSingleHandler;
            _usageLevel4GetSingleHandler.NotNull(nameof(UsageLevel4GetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageLevel4GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var usageLevel4s = await _usageLevel4GetSingleHandler.Handle(id, cancellationToken);
            return Ok(usageLevel4s);
        }
    }
}
