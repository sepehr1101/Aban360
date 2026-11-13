using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    public class AccountTypeController : BaseController
    {
        private readonly IAccountTypeGetAllHandler _c0_accountTypeGetAllHandler;
        private readonly IAccountTypeGetSingleHandler _c1_accountTypeGetSingleHandler;
        public AccountTypeController(IAccountTypeGetAllHandler c0_accountTypeGetAllHandler, IAccountTypeGetSingleHandler c1_accountTypeGetSingleHandler)
        {
            _c0_accountTypeGetAllHandler = c0_accountTypeGetAllHandler;
            _c0_accountTypeGetAllHandler.NotNull(nameof(c0_accountTypeGetAllHandler));
            _c1_accountTypeGetSingleHandler = c1_accountTypeGetSingleHandler;
            _c1_accountTypeGetSingleHandler.NotNull(nameof(c1_accountTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/account-type/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<AccountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var accountTypes = await _c0_accountTypeGetAllHandler.Handle(cancellationToken);
            return Ok(accountTypes);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/account-type/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AccountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(AccountTypeEnum id, CancellationToken cancellationToken)
        {
            var accountTypes = await _c1_accountTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(accountTypes);
        }
    }
}
