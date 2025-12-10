using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Commands.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP.Commands
{
    [Route("v1/tax")]
    public class TaxCreateListController : BaseController
    {
        private readonly INewListCreateHandler _sentInvoiceHandler;
        public TaxCreateListController(INewListCreateHandler sentInvoiceHandler)
        {
            _sentInvoiceHandler = sentInvoiceHandler;
            _sentInvoiceHandler.NotNull(nameof(sentInvoiceHandler));
        }

        [HttpPost]
        [Route("create-list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateList(NewListCreateDto inputDto, CancellationToken cancellationToken)
        {
            int newWrapperId = await _sentInvoiceHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(newWrapperId);
        }
    }
}
