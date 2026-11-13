using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    public class CreditorTypeController : BaseController
    {
        private readonly ICreditorTypeService _service;

        public CreditorTypeController(ICreditorTypeService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("~/v1/creditor-type/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CreditorTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var creditorTypes = await _service.GetAllAsync(cancellationToken);
            return Ok(creditorTypes);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/creditor-type/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditorTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(CreditorTypeEnum id, CancellationToken cancellationToken)
        {
            var creditorTypes = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(creditorTypes);
        }
    }
}
