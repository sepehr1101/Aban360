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
    [Route("v1/water-return-summary")]
    public class WaterReturnSummaryController : BaseController
    {
        private readonly IWaterReturnSummaryHandler _waterReturnSummary;
        private readonly IReportGenerator _reportGenerator;
        public WaterReturnSummaryController(
            IWaterReturnSummaryHandler waterReturnSummary,
            IReportGenerator reportGenerator)
        {
            _waterReturnSummary = waterReturnSummary;
            _waterReturnSummary.NotNull(nameof(_waterReturnSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterReturnSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto> waterReturn = await _waterReturnSummary.Handle(inputDto, cancellationToken);
            return Ok(waterReturn);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterReturnSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportTitle = GetReportTitle(inputDto.EnumInput);
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterReturnSummary.Handle, CurrentUser, ReportLiterals.WaterReturnSummary + reportTitle, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WaterReturnSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.WaterReturnSummary;
            ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto> calculationDetails = await _waterReturnSummary.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }

        private string GetReportTitle(WaterReturnSummaryEnum enumState)
        {
            string baseReportTitle = " بر اساس ";
            Dictionary<int, string> titles = new Dictionary<int, string>()
            {
                { 0,"کاربری"},
                { 1,"ناحیه"},
                { 2,"روز"},
                { 4,"میانگین مصرف"},
                { 5,"منطقه"},
                { 6,"کاربری و ناحیه"},
            };
            return baseReportTitle + titles[(int)enumState];
        }
    }
}
