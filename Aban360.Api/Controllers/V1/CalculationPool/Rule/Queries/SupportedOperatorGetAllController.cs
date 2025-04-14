using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/supported-operator")]
    public class SupportedOperatorGetAllController : BaseController
    {
        private readonly ISupportedOperatorGetAllHandler _supportedOperatorGetAllHandler;
        public SupportedOperatorGetAllController(ISupportedOperatorGetAllHandler supportedOperatorGetAllHandler)
        {
            _supportedOperatorGetAllHandler = supportedOperatorGetAllHandler;
            _supportedOperatorGetAllHandler.NotNull(nameof(supportedOperatorGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SupportedOperatorGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var supportedOperators = await _supportedOperatorGetAllHandler.Handle(cancellationToken);
            return Ok(supportedOperators);
        }
    }
}
