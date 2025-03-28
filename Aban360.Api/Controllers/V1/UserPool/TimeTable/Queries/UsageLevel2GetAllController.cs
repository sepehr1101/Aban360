using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v2/usage-level-2")]
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
