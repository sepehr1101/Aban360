using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/bank-account")]
    public class BankAccountGetSingleController : BaseController
    {
        private readonly IBankAccountGetSingleHandler _bankAccountGetSingleHandler;
        public BankAccountGetSingleController(IBankAccountGetSingleHandler bankAccountGetSingleHandler)
        {
            _bankAccountGetSingleHandler = bankAccountGetSingleHandler;
            _bankAccountGetSingleHandler.NotNull(nameof(bankAccountGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var bankAccounts = await _bankAccountGetSingleHandler.Handle(id, cancellationToken);
            return Ok(bankAccounts);
        }
    }

}
