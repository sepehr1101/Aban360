using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/bank-account")]
    public class BankAccountGetAllController : BaseController
    {
        private readonly IBankAccountGetAllHandler _bankAccountGetAllHandler;
        public BankAccountGetAllController(IBankAccountGetAllHandler bankAccountGetAllHandler)
        {
            _bankAccountGetAllHandler = bankAccountGetAllHandler;
            _bankAccountGetAllHandler.NotNull(nameof(bankAccountGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankAccountGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var bankAccounts = await _bankAccountGetAllHandler.Handle(cancellationToken);
            return Ok(bankAccounts);
        }
    }

}
