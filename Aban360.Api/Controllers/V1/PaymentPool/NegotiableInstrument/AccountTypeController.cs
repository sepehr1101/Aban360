using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    public class AccountTypeController : BaseController
    {
        private readonly IAccountTypeService _service;

        public AccountTypeController(IAccountTypeService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("~/v1/account-type/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<AccountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var accountTypes = await _service.GetAllAsync(cancellationToken);
            return Ok(accountTypes);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/account-type/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AccountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(AccountTypeEnum id, CancellationToken cancellationToken)
        {
            var accountTypes = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(accountTypes);
        }
    }
}
