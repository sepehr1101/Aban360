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
    [Route("v1/water-discount")]
    public class WaterDiscountController : BaseController
    {
        private readonly IWaterDiscountDetailHandler _WaterDiscountDetailHandler;
        private readonly IWaterDiscountSummaryHandler _WaterDiscountSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterDiscountController(
            IWaterDiscountDetailHandler WaterDiscountDetailHandler,
            IWaterDiscountSummaryHandler WaterDiscountSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _WaterDiscountDetailHandler = WaterDiscountDetailHandler;
            _WaterDiscountDetailHandler.NotNull(nameof(WaterDiscountDetailHandler));

            _WaterDiscountSummaryHandler = WaterDiscountSummaryHandler;
            _WaterDiscountSummaryHandler.NotNull(nameof(WaterDiscountSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("detail-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailRaw(WaterDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto> WaterDiscountDetail = await _WaterDiscountDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(WaterDiscountDetail);
        }

        [HttpPost, HttpGet]
        [Route("detail-excel/{connectionId}")]
        public async Task<IActionResult> GetDetailExcel(string connectionId, WaterDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _WaterDiscountDetailHandler.Handle, CurrentUser, ReportLiterals.WaterDiscountDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("detail-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailStiReport(WaterDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2110;
            ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto> result = await _WaterDiscountDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }

        [HttpPost, HttpGet]
        [Route("summary-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetsummaryRaw(WaterDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> WaterDiscountsummary = await _WaterDiscountSummaryHandler.Handle(inputDto, cancellationToken);
            return Ok(WaterDiscountsummary);
        }

        [HttpPost, HttpGet]
        [Route("summary-excel/{connectionId}")]
        public async Task<IActionResult> GetsummaryExcel(string connectionId, WaterDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _WaterDiscountSummaryHandler.Handle, CurrentUser, ReportLiterals.WaterDiscountSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("summary-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetsummaryStiReport(WaterDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2111;
            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> result = await _WaterDiscountSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
