using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP.Queries
{
    [Route("v1/maahertsp-invoice")]
    public class SentInvoiceController : BaseController
    {
        private readonly ISentInvoiceHandler _sentInvoiceHandler;
        public SentInvoiceController(ISentInvoiceHandler sentInvoiceHandler)
        {
            _sentInvoiceHandler = sentInvoiceHandler;
            _sentInvoiceHandler.NotNull(nameof(sentInvoiceHandler));
        }

        [HttpPost]
        [Route("sent")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SentInvoiceRecieveDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Send(ICollection<MaaherTSPInvoiceDto> inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<SentInvoiceRecieveDto> sentInvoiceRecieve = await _sentInvoiceHandler.Handle(inputDto, cancellationToken);
            return Ok(sentInvoiceRecieve);
        }
    }
}
