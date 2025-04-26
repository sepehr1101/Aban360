using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/account-type")]
    public class AccountTypeGetAllController : BaseController
    {
        private readonly IAccountTypeGetAllHandler _accountTypeGetAllHandler;
        public AccountTypeGetAllController(IAccountTypeGetAllHandler accountTypeGetAllHandler)
        {
            _accountTypeGetAllHandler = accountTypeGetAllHandler;
            _accountTypeGetAllHandler.NotNull(nameof(accountTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<AccountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var accountTypes = await _accountTypeGetAllHandler.Handle(cancellationToken);
            return Ok(accountTypes);
        }
    }
}
