using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/handover")]
    public class HandoverGetSingleController : BaseController
    {
        private readonly IHandoverGetSingleHandler _handoverGetSingleHandler;
        public HandoverGetSingleController(IHandoverGetSingleHandler handoverGetSingleHandler)
        {
            _handoverGetSingleHandler = handoverGetSingleHandler;
            _handoverGetSingleHandler.NotNull(nameof(handoverGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var handovers = await _handoverGetSingleHandler.Handle(id, cancellationToken);
            return Ok(handovers);
        }
    }
}
