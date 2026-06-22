using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingUpdatedController : BaseController
    {
        private readonly IMeterReadingDetailUpdatedGetHadler _updatedGetHandler;
        public MeterReadingUpdatedController(IMeterReadingDetailUpdatedGetHadler updatedGetHandler)
        {
            _updatedGetHandler = updatedGetHandler;
            _updatedGetHandler.NotNull(nameof(updatedGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("updated")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpdated([FromBody] MeterReadingDetailUpdatedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto> result = await _updatedGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
