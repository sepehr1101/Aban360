using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Microsoft.AspNetCore.Mvc;
using IMeterReadingFileReportsService = global::Aban360.CalculationPool.Application.Features.MeterReading.Services.IMeterReadingFileReportsService;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingUploadedToRegisteredController : BaseController
    {
        private readonly IMeterReadingFileReportsService _meterReadingFileReportsService;
        private readonly IReportGenerator _reportGenerator;
        public MeterReadingUploadedToRegisteredController(IMeterReadingFileReportsService meterReadingFileReportsService, IReportGenerator reportGenerator)
        {
            _meterReadingFileReportsService = meterReadingFileReportsService;
            _meterReadingFileReportsService.NotNull(nameof(meterReadingFileReportsService));
            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("uploaded-to-registered-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto> result = await _meterReadingFileReportsService.MeterReadingUploadedToRegisteredSummaryByZoneGetAsync(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("uploaded-to-registered-excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingUploadToRegisteredInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterReadingFileReportsService.MeterReadingUploadedToRegisteredSummaryByZoneGetAsync, CurrentUser, ReportLiterals.MeterReadingUploadedToRegistered, connectionId);
            return Ok(inputDto);
        }
    }
}
