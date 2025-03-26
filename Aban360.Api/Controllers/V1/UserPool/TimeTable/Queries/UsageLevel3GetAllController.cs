using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
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
