using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/malfunction-meter-by-duration-summary-by-usage")]
    public class MalfunctionMeterByDurationSummaryByUsageController : BaseController
    {
        private readonly IMalfunctionMeterByDurationSummaryByUsageHandler _malfunctionMeterByDurationSummaryByUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionMeterByDurationSummaryByUsageController(
            IMalfunctionMeterByDurationSummaryByUsageHandler malfunctionMeterByDurationSummaryByUsageHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionMeterByDurationSummaryByUsageHandler = malfunctionMeterByDurationSummaryByUsageHandler;
            _malfunctionMeterByDurationSummaryByUsageHandler.NotNull(nameof(malfunctionMeterByDurationSummaryByUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto> result = await _malfunctionMeterByDurationSummaryByUsageHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionMeterByDurationInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionMeterByDurationSummaryByUsageHandler.Handle, CurrentUser, ReportLiterals.MalfunctionMeterByDurationSummary + ReportLiterals.ByUsage, connectionId);
            return Ok(inputDto);
        }
    }
}
