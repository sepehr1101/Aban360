using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/creditor-type")]
    public class CreditorTypeGetSingleController : BaseController
    {
        private readonly ICreditorTypeGetSingleHandler _creditorTypeGetSingleHandler;
        public CreditorTypeGetSingleController(ICreditorTypeGetSingleHandler creditorTypeGetSingleHandler)
        {
            _creditorTypeGetSingleHandler = creditorTypeGetSingleHandler;
            _creditorTypeGetSingleHandler.NotNull(nameof(creditorTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditorTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(CreditorTypeEnum id, CancellationToken cancellationToken)
        {
            var creditorTypes = await _creditorTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(creditorTypes);
        }
    }
}
