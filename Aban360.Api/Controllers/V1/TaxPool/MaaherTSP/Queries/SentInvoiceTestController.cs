using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP.Queries
{
    [Route("v1/tax")]
    public class SentInvoiceTestController : BaseController
    {
        private readonly ISentInvoiceHandler _sentInvoiceHandler;
        public SentInvoiceTestController(ISentInvoiceHandler sentInvoiceHandler)
        {
            _sentInvoiceHandler = sentInvoiceHandler;
            _sentInvoiceHandler.NotNull(nameof(sentInvoiceHandler));
        }

        [HttpPost]
        [Route("send-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MaaherResponseNew>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendManual(ICollection<MaaherRequestWrapper_New> inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MaaherResponseNew> sentInvoiceRecieve = await _sentInvoiceHandler.Handle(inputDto, cancellationToken);
            return Ok(sentInvoiceRecieve);
        }


        [HttpPost]
        [Route("confirm/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MaaherResponseNew>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm(int id, CancellationToken cancellationToken)
        {
            IEnumerable<MaaherResponseNew> sentInvoiceRecieve = await _sentInvoiceHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(sentInvoiceRecieve);
        }
    }
}
