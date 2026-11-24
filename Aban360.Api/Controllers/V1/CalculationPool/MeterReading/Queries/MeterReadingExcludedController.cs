using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;
using IMeterReadingChangeReportsService = global::Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingChangeReportsService;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingExcludedController : BaseController
    {
        private readonly IMeterReadingChangeReportsService _meterReadingChangeReportsService;
        public MeterReadingExcludedController(IMeterReadingChangeReportsService meterReadingChangeReportsService)
        {
            _meterReadingChangeReportsService = meterReadingChangeReportsService;
            _meterReadingChangeReportsService.NotNull(nameof(meterReadingChangeReportsService));
        }

        [HttpPost]
        [Route("excluded")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcluded([FromBody] MeterReadingDetailExcludedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto> result = await _meterReadingChangeReportsService.MeterReadingDetailExcludedGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("excluded-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiExcluded([FromBody] MeterReadingDetailExcludedInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.MeterReadingExcluded;
            ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto> result = await _meterReadingChangeReportsService.MeterReadingDetailExcludedGetAsync(inputDto, CurrentUser, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode, true);
            return Ok(reportId);
        }
    }
}
