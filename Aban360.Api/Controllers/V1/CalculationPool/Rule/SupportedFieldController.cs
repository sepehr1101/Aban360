using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/supported-field")]
    public class SupportedFieldController : BaseController
    {
        private readonly ISupportedFieldGetSingleHandler _supportedFieldGetSingleHandler;
        private readonly ISupportedFieldGetAllHandler _supportedFieldGetAllHandler;

        public SupportedFieldController(
            ISupportedFieldGetSingleHandler supportedFieldGetSingleHandler,
            ISupportedFieldGetAllHandler supportedFieldGetAllHandler)
        {
            _supportedFieldGetSingleHandler = supportedFieldGetSingleHandler;
            _supportedFieldGetSingleHandler.NotNull(nameof(supportedFieldGetSingleHandler));

            _supportedFieldGetAllHandler = supportedFieldGetAllHandler;
            _supportedFieldGetAllHandler.NotNull(nameof(supportedFieldGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SupportedFieldGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var supportedFields = await _supportedFieldGetSingleHandler.Handle(id, cancellationToken);
            return Ok(supportedFields);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SupportedFieldGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var supportedFields = await _supportedFieldGetAllHandler.Handle(cancellationToken);
            return Ok(supportedFields);
        }
    }
}
