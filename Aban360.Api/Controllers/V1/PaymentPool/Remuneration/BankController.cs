using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    public class BankController : BaseController
    {
        private readonly IBankGetAllHandler _c0_bankGetAllHandler;
        private readonly IBankGetSingleHandler _c1_bankGetSingleHandler;
        public BankController(IBankGetAllHandler c0_bankGetAllHandler, IBankGetSingleHandler c1_bankGetSingleHandler)
        {
            _c0_bankGetAllHandler = c0_bankGetAllHandler;
            _c0_bankGetAllHandler.NotNull(nameof(c0_bankGetAllHandler));
            _c1_bankGetSingleHandler = c1_bankGetSingleHandler;
            _c1_bankGetSingleHandler.NotNull(nameof(c1_bankGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bank/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var banks = await _c0_bankGetAllHandler.Handle(cancellationToken);
            return Ok(banks);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bank/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var banks = await _c1_bankGetSingleHandler.Handle(id, cancellationToken);
            return Ok(banks);
        }
    }
}
