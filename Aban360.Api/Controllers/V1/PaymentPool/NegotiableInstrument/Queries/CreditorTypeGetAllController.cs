using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/creditor-type")]
    public class CreditorTypeGetAllController : BaseController
    {
        private readonly ICreditorTypeGetAllHandler _creditorTypeGetAllHandler;
        public CreditorTypeGetAllController(ICreditorTypeGetAllHandler creditorTypeGetAllHandler)
        {
            _creditorTypeGetAllHandler = creditorTypeGetAllHandler;
            _creditorTypeGetAllHandler.NotNull(nameof(creditorTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CreditorTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var creditorTypes = await _creditorTypeGetAllHandler.Handle(cancellationToken);
            return Ok(creditorTypes);
        }
    }
}
