using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Commands.Contracts;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP.Commands
{
    [Route("v1/maahertsp-invoice")]
    public class CreateInvoiceController : BaseController
    {
        private readonly INewListCreateHandler _sentInvoiceHandler;
        public CreateInvoiceController(INewListCreateHandler sentInvoiceHandler)
        {
            _sentInvoiceHandler = sentInvoiceHandler;
            _sentInvoiceHandler.NotNull(nameof(sentInvoiceHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SentInvoiceRecieveDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(NewListCreateDto inputDto, CancellationToken cancellationToken)
        {
            await _sentInvoiceHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
