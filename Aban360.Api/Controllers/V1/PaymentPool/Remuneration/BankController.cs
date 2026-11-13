using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Services;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    public class BankController : BaseController
    {
        private readonly IBankService _service;

        public BankController(IBankService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bank/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var banks = await _service.GetAllAsync(cancellationToken);
            return Ok(banks);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bank/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var banks = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(banks);
        }
    }
}
