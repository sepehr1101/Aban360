using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using IMeterReadingChangeReportsService = global::Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingChangeReportsService;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingUpdatedController : BaseController
    {
        private readonly IMeterReadingChangeReportsService _meterReadingChangeReportsService;
        public MeterReadingUpdatedController(IMeterReadingChangeReportsService meterReadingChangeReportsService)
        {
            _meterReadingChangeReportsService = meterReadingChangeReportsService;
            _meterReadingChangeReportsService.NotNull(nameof(meterReadingChangeReportsService));
        }

        [HttpGet, HttpPost]
        [Route("updated")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpdated([FromBody] MeterReadingDetailUpdatedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto> result = await _meterReadingChangeReportsService.MeterReadingDetailUpdatedGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
