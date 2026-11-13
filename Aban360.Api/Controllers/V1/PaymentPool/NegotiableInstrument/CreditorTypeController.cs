using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    public class CreditorTypeController : BaseController
    {
        private readonly ICreditorTypeGetAllHandler _c0_creditorTypeGetAllHandler;
        private readonly ICreditorTypeGetSingleHandler _c1_creditorTypeGetSingleHandler;
        public CreditorTypeController(ICreditorTypeGetAllHandler c0_creditorTypeGetAllHandler, ICreditorTypeGetSingleHandler c1_creditorTypeGetSingleHandler)
        {
            _c0_creditorTypeGetAllHandler = c0_creditorTypeGetAllHandler;
            _c0_creditorTypeGetAllHandler.NotNull(nameof(c0_creditorTypeGetAllHandler));
            _c1_creditorTypeGetSingleHandler = c1_creditorTypeGetSingleHandler;
            _c1_creditorTypeGetSingleHandler.NotNull(nameof(c1_creditorTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/creditor-type/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CreditorTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var creditorTypes = await _c0_creditorTypeGetAllHandler.Handle(cancellationToken);
            return Ok(creditorTypes);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/creditor-type/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditorTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(CreditorTypeEnum id, CancellationToken cancellationToken)
        {
            var creditorTypes = await _c1_creditorTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(creditorTypes);
        }
    }
}
