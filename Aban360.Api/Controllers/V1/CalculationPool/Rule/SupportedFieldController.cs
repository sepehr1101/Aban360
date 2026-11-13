using Aban360.CalculationPool.Application.Features.Rule.Services;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/supported-field")]
    public class SupportedFieldController : BaseController
    {
        private readonly ISupportedFieldService _service;

        public SupportedFieldController(ISupportedFieldService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SupportedFieldGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var supportedFields = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(supportedFields);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SupportedFieldGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var supportedFields = await _service.GetAllAsync(cancellationToken);
            return Ok(supportedFields);
        }
    }
}
