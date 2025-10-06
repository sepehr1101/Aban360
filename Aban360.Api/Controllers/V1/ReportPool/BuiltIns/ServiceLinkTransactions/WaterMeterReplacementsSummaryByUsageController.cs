using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/water-meter-replacements-summary-by-usage")]
    public class WaterMeterReplacementsSummaryByUsageController : BaseController
    {
        private readonly IWaterMeterReplacementsSummaryByUsageHandler _waterMeterReplacementsSummaryByUsageHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterMeterReplacementsSummaryByUsageController(
            IWaterMeterReplacementsSummaryByUsageHandler waterMeterReplacementsSummaryByUsageHandler,
            IReportGenerator reportGenerator)
        {
            _waterMeterReplacementsSummaryByUsageHandler = waterMeterReplacementsSummaryByUsageHandler;
            _waterMeterReplacementsSummaryByUsageHandler.NotNull(nameof(waterMeterReplacementsSummaryByUsageHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterMeterReplacementsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto> waterMeterReplacementsSummaryByUsage = await _waterMeterReplacementsSummaryByUsageHandler.Handle(input, cancellationToken);
            return Ok(waterMeterReplacementsSummaryByUsage);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterMeterReplacementsInputDto inputDto, CancellationToken cancellationToken)
        {
            string stateTitle = inputDto.IsChangeDate ? ReportLiterals.ChangeDate : ReportLiterals.RegisterDate;
            string reportTitle = ReportLiterals.WaterMeterReplacementsSummary(stateTitle) + ReportLiterals.ByUsage;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterMeterReplacementsSummaryByUsageHandler.Handle, CurrentUser, reportTitle, connectionId);
            return Ok(inputDto);
        }
    }
}
