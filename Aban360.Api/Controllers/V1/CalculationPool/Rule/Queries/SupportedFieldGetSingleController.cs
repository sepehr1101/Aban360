using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule.Queries
{
    [Route("v1/supported-field")]
    public class SupportedFieldGetSingleController : BaseController
    {
        private readonly ISupportedFieldGetSingleHandler _supportedFieldGetSingleHandler;
        public SupportedFieldGetSingleController(ISupportedFieldGetSingleHandler supportedFieldGetSingleHandler)
        {
            _supportedFieldGetSingleHandler = supportedFieldGetSingleHandler;
            _supportedFieldGetSingleHandler.NotNull(nameof(supportedFieldGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SupportedFieldGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var supportedFields = await _supportedFieldGetSingleHandler.Handle(id, cancellationToken);
            return Ok(supportedFields);
        }
    }
}
