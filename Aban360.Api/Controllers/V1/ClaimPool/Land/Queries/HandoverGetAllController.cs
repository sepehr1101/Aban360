using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/handover")]
    public class HandoverGetAllController : BaseController
    {
        private readonly IHandoverGetAllHandler _handoverGetAllHandler;
        public HandoverGetAllController(IHandoverGetAllHandler handoverGetAllHandler)
        {
            _handoverGetAllHandler = handoverGetAllHandler;
            _handoverGetAllHandler.NotNull(nameof(handoverGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HandoverGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var handovers = await _handoverGetAllHandler.Handle(cancellationToken);
            return Ok(handovers);
        }
    }
}
