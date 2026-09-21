using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/bill")]
    public class InvalidPaymentIdController : BaseController
    {
        private readonly IInvalidPaymentIdGetHandler _invalidPaymentIdGetHandler;
        public InvalidPaymentIdController(IInvalidPaymentIdGetHandler invalidPaymentIdGetHandler)
        {
            _invalidPaymentIdGetHandler = invalidPaymentIdGetHandler;
            _invalidPaymentIdGetHandler.NotNull(nameof(invalidPaymentIdGetHandler));
        }

        [HttpPost, HttpGet]
        [Route("invalid-payment-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] InvalidPaymentIdInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto> companyServiceTypes = await _invalidPaymentIdGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
