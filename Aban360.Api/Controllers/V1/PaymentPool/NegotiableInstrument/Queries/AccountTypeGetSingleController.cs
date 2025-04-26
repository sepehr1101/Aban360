using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/account-type")]
    public class AccountTypeGetSingleController : BaseController
    {
        private readonly IAccountTypeGetSingleHandler _accountTypeGetSingleHandler;
        public AccountTypeGetSingleController(IAccountTypeGetSingleHandler accountTypeGetSingleHandler)
        {
            _accountTypeGetSingleHandler = accountTypeGetSingleHandler;
            _accountTypeGetSingleHandler.NotNull(nameof(accountTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AccountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(AccountTypeEnum id, CancellationToken cancellationToken)
        {
            var accountTypes = await _accountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(accountTypes);
        }
    }
}
