using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-net-sales-detail")]
    public class WaterNetSalesDetailController : BaseController
    {
        private readonly IWaterNetSalesDetailHandler _waterNetSalesDetail;
        private readonly IReportGenerator _reportGenerator;
        public WaterNetSalesDetailController(
            IWaterNetSalesDetailHandler waterNetSalesDetail,
            IReportGenerator reportGenerator)
        {
            _waterNetSalesDetail = waterNetSalesDetail;
            _waterNetSalesDetail.NotNull(nameof(_waterNetSalesDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto> waterSales = await _waterNetSalesDetail.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterNetSalesDetail.Handle, CurrentUser, ReportLiterals.WaterNetSalesDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 530;
            ReportOutput<WaterSalesHeaderOutputDto, WaterNetRawSalesDetailDataOutputDto> result = await _waterNetSalesDetail.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
