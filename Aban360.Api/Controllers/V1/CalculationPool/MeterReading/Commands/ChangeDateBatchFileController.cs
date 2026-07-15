using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-file")]
    public class ChangeDateBatchFileController : BaseController
    {
        private readonly IChangeDateBatchHandler _changeDateBatchHandler;
        public ChangeDateBatchFileController(IChangeDateBatchHandler changeDateBatchHandler)
        {
            _changeDateBatchHandler = changeDateBatchHandler;
            _changeDateBatchHandler.NotNull(nameof(changeDateBatchHandler));
        }

        [HttpPost]
        [Route("change-date-batch")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ChangeDateBatchOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeDate([FromBody] ChangeDateBatchInputDto input, CancellationToken cancellationToken)
        {
            ChangeDateBatchOutputDto result = await _changeDateBatchHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
