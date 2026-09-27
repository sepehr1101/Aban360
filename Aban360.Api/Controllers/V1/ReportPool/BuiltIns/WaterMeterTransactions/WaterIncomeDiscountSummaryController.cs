using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-income-discount-summary")]
    public class WaterIncomeDiscountSummaryController : BaseController
    {
        private readonly IWaterIncomeDiscountSummaryHandler _waterIncomeDiscountSummary;
        private readonly IReportGenerator _reportGenerator;
        public WaterIncomeDiscountSummaryController(
            IWaterIncomeDiscountSummaryHandler waterIncomeDiscountSummary,
            IReportGenerator reportGenerator)
        {
            _waterIncomeDiscountSummary = waterIncomeDiscountSummary;
            _waterIncomeDiscountSummary.NotNull(nameof(_waterIncomeDiscountSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterIncomeDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto> waterIncomeDiscount = await _waterIncomeDiscountSummary.Handle(inputDto, cancellationToken);
            return Ok(waterIncomeDiscount);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterIncomeDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportTitle = GetReportTitle(inputDto.SummaryType);
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterIncomeDiscountSummary.Handle, CurrentUser, ReportLiterals.WaterIncomeDiscountSummary + reportTitle, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WaterIncomeDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.WaterIncomeDiscountSummary;
            ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto> calculationDetails = await _waterIncomeDiscountSummary.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }

        private string GetReportTitle(WaterIncomeDiscountSummaryEnum enumState)
        {
            string baseReportTitle = " بر اساس ";
            Dictionary<int, string> titles = new Dictionary<int, string>()
            {
                { 1,"ناحیه"},
                { 2,"منطقه"},
            };
            return baseReportTitle + titles[(int)enumState];
        }
    }
}
