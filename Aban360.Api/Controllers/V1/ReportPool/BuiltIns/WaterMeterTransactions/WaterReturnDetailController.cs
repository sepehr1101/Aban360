using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-return-detail")]
    public class WaterReturnDetailController : BaseController
    {
        private readonly IWaterReturnDetailHandler _waterReturnDetail;
        private readonly IReportGenerator _reportGenerator;
        public WaterReturnDetailController(
            IWaterReturnDetailHandler waterReturnDetail,
            IReportGenerator reportGenerator)
        {
            _waterReturnDetail = waterReturnDetail;
            _waterReturnDetail.NotNull(nameof(_waterReturnDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterReturnDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterReturnDetailHeaderOutputDto, WaterReturnDetailDataOutputDto> waterReturn = await _waterReturnDetail.Handle(inputDto, cancellationToken);
            return Ok(waterReturn);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterReturnDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterReturnDetail.Handle, CurrentUser, ReportLiterals.WaterReturnDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WaterReturnDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.WaterReturnDetail;
            ReportOutput<WaterReturnDetailHeaderOutputDto, WaterReturnDetailDataOutputDto> calculationDetails = await _waterReturnDetail.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
