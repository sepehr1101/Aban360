using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/handover")]
    public class HandoverGetAllController : BaseController
    {
        private readonly IHandoverGetAllHandler _handoverGetAllHandler;
        private readonly IHandoverHandler _handoverHandler;
        public HandoverGetAllController(
            IHandoverGetAllHandler handoverGetAllHandler,
            IHandoverHandler handoverHandler)
        {
            _handoverGetAllHandler = handoverGetAllHandler;
            _handoverGetAllHandler.NotNull(nameof(handoverGetAllHandler));

            _handoverHandler = handoverHandler;
            _handoverHandler.NotNull(nameof(handoverHandler));
        }

        [HttpPost, HttpGet]
        [Route("all-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HandoverGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var handovers = await _handoverGetAllHandler.Handle(cancellationToken);
            return Ok(handovers);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HandoverQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            ICollection<HandoverQueryDto> handovers = (await _handoverHandler.Handle(cancellationToken)).ToList();
            return Ok(handovers);
        }
    }
}
