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
        private readonly IWaterDiscountDetailHandler _waterDiscountDetailHandler;
        private readonly IWaterDiscountByDiscountTypeDetailHandler _waterDiscountByDiscountTypeDetailHandler;
        private readonly IWaterDiscountSummaryHandler _waterDiscountSummaryHandler;
        private readonly IWaterDiscountByDiscountTypeSummaryHandler _waterDiscountByDiscountTypeSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterDiscountController(
            IWaterDiscountDetailHandler waterDiscountDetailHandler,
            IWaterDiscountByDiscountTypeDetailHandler waterDiscountByDiscountTypeDetailHandler,
            IWaterDiscountSummaryHandler waterDiscountSummaryHandler,
            IWaterDiscountByDiscountTypeSummaryHandler waterDiscountByDiscountTypeSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _waterDiscountDetailHandler = waterDiscountDetailHandler;
            _waterDiscountDetailHandler.NotNull(nameof(waterDiscountDetailHandler));

            _waterDiscountByDiscountTypeDetailHandler = waterDiscountByDiscountTypeDetailHandler;
            _waterDiscountByDiscountTypeDetailHandler.NotNull(nameof(waterDiscountByDiscountTypeDetailHandler));

            _waterDiscountSummaryHandler = waterDiscountSummaryHandler;
            _waterDiscountSummaryHandler.NotNull(nameof(waterDiscountSummaryHandler));

            _waterDiscountByDiscountTypeSummaryHandler = waterDiscountByDiscountTypeSummaryHandler;
            _waterDiscountByDiscountTypeSummaryHandler.NotNull(nameof(waterDiscountByDiscountTypeSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("detail-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailRaw(WaterDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto> WaterDiscountDetail = await _waterDiscountDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(WaterDiscountDetail);
        }

        [HttpPost, HttpGet]
        [Route("detail-excel/{connectionId}")]
        public async Task<IActionResult> GetDetailExcel(string connectionId, WaterDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterDiscountDetailHandler.Handle, CurrentUser, ReportLiterals.WaterDiscountDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("detail-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailStiReport(WaterDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2110;
            ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto> result = await _waterDiscountDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }


        ////////////////////////////////
        [HttpPost, HttpGet]
        [Route("detail-by-discount-type-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailByDiscountTypeRaw(WaterDiscountByTypeDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto> WaterDiscountDetail = await _waterDiscountByDiscountTypeDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(WaterDiscountDetail);
        }

        [HttpPost, HttpGet]
        [Route("detail-by-discount-type-excel/{connectionId}")]
        public async Task<IActionResult> GetDetailByDiscountTypeExcel(string connectionId, WaterDiscountByTypeDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterDiscountByDiscountTypeDetailHandler.Handle, CurrentUser, ReportLiterals.WaterDiscountDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("detail-by-discount-type-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailByDiscountTypeStiReport(WaterDiscountByTypeDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2110;
            ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto> result = await _waterDiscountByDiscountTypeDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }


        ////////////////////////////////
        [HttpPost, HttpGet]
        [Route("summary-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetsummaryRaw(WaterDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> WaterDiscountsummary = await _waterDiscountSummaryHandler.Handle(inputDto, cancellationToken);
            return Ok(WaterDiscountsummary);
        }

        [HttpPost, HttpGet]
        [Route("summary-excel/{connectionId}")]
        public async Task<IActionResult> GetsummaryExcel(string connectionId, WaterDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterDiscountSummaryHandler.Handle, CurrentUser, ReportLiterals.WaterDiscountSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("summary-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetsummaryStiReport(WaterDiscountSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2111;
            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> result = await _waterDiscountSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    
        
        ////////////////////////////////
        [HttpPost, HttpGet]
        [Route("summary-by-discount-type-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryByDiscountTypeRaw(WaterDiscountByTypeSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> WaterDiscountsummary = await _waterDiscountByDiscountTypeSummaryHandler.Handle(inputDto, cancellationToken);
            return Ok(WaterDiscountsummary);
        }

        [HttpPost, HttpGet]
        [Route("summary-by-discount-type-excel/{connectionId}")]
        public async Task<IActionResult> GetSummaryByDiscountTypeExcel(string connectionId, WaterDiscountByTypeSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterDiscountByDiscountTypeSummaryHandler.Handle, CurrentUser, ReportLiterals.WaterDiscountSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("summary-by-discount-type-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryByDiscountTypeStiReport(WaterDiscountByTypeSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2111;
            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> result = await _waterDiscountByDiscountTypeSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    
    
    }
}
