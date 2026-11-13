using Aban360.CalculationPool.Application.Features.Rule.Services;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/supported-operator")]
    public class SupportedOperatorController : BaseController
    {
        private readonly ISupportedOperatorService _service;

        public SupportedOperatorController(ISupportedOperatorService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SupportedOperatorGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var supportedOperators = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(supportedOperators);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SupportedOperatorGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var supportedOperators = await _service.GetAllAsync(cancellationToken);
            return Ok(supportedOperators);
        }
    }
}
