using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingUploadedToRegisteredController : BaseController
    {
        private readonly IMeterReadingUploadedToRegisteredSummaryByZoneGetHandler _meterReadingUploadedToRegisteredGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterReadingUploadedToRegisteredController(
            IMeterReadingUploadedToRegisteredSummaryByZoneGetHandler meterReadingUploadedToRegisteredGetHandler,
            IReportGenerator reportGenerator)
        {
            _meterReadingUploadedToRegisteredGetHandler = meterReadingUploadedToRegisteredGetHandler;
            _meterReadingUploadedToRegisteredGetHandler.NotNull(nameof(meterReadingUploadedToRegisteredGetHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpGet, HttpPost]
        [Route("uploaded-to-registered-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto> result = await _meterReadingUploadedToRegisteredGetHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("uploaded-to-registered-excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingUploadToRegisteredInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterReadingUploadedToRegisteredGetHandler.Handle, CurrentUser, ReportLiterals.MeterReadingUploadedToRegistered, connectionId);
            return Ok(inputDto);
        }
    }
}
