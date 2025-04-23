using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    [Route("v1/bank")]
    public class BankGetAllController : BaseController
    {
        private readonly IBankGetAllHandler _bankGetAllHandler;
        public BankGetAllController(IBankGetAllHandler bankGetAllHandler)
        {
            _bankGetAllHandler = bankGetAllHandler;
            _bankGetAllHandler.NotNull(nameof(bankGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var banks = await _bankGetAllHandler.Handle(cancellationToken);
            return Ok(banks);
        }
    }

}
