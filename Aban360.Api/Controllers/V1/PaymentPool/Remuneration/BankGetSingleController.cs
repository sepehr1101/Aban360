using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    [Route("v1/bank")]
    public class BankGetSingleController : BaseController
    {
        private readonly IBankGetSingleHandler _bankGetSingleHandler;
        public BankGetSingleController(IBankGetSingleHandler bankGetSingleHandler)
        {
            _bankGetSingleHandler = bankGetSingleHandler;
            _bankGetSingleHandler.NotNull(nameof(bankGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var banks = await _bankGetSingleHandler.Handle(id, cancellationToken);
            return Ok(banks);
        }
    }

}
