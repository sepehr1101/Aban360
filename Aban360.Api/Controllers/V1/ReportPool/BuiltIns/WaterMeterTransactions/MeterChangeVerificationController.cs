using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/meter-change-varification")]
    public class MeterChangeVerificationController : BaseController
    {
        private readonly IMeterChangeVerificationDetailHandler _meterChangeVerificationDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public MeterChangeVerificationController(
            IMeterChangeVerificationDetailHandler meterChangeVerificationDetailHandler,
            IReportGenerator reportGenerator)
        {
            _meterChangeVerificationDetailHandler = meterChangeVerificationDetailHandler;
            _meterChangeVerificationDetailHandler.NotNull(nameof(meterChangeVerificationDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] MeterChangeVerificationInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto> result = await _meterChangeVerificationDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] MeterChangeVerificationInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterChangeVerificationDetailHandler.Handle, CurrentUser, ReportLiterals.MeterChangeVerification, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(MeterChangeVerificationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.MeterChangeVerification;
            ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto> calculationDetails = await _meterChangeVerificationDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
