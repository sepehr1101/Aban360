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
    [Route("v1/water-income-discount-detail")]
    public class WaterIncomeDiscountDetailController : BaseController
    {
        private readonly IWaterIncomeDiscountDetailHandler _waterIncomeDiscountDetail;
        private readonly IReportGenerator _reportGenerator;
        public WaterIncomeDiscountDetailController(
            IWaterIncomeDiscountDetailHandler waterIncomeDiscountDetail,
            IReportGenerator reportGenerator)
        {
            _waterIncomeDiscountDetail = waterIncomeDiscountDetail;
            _waterIncomeDiscountDetail.NotNull(nameof(_waterIncomeDiscountDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterIncomeDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterIncomeDiscountDetailHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto> waterIncomeDiscount = await _waterIncomeDiscountDetail.Handle(inputDto, cancellationToken);
            return Ok(waterIncomeDiscount);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterIncomeDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterIncomeDiscountDetail.Handle, CurrentUser, ReportLiterals.WaterIncomeDiscountDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WaterIncomeDiscountDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.WaterIncomeDiscountDetail;
            ReportOutput<WaterIncomeDiscountDetailHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto> calculationDetails = await _waterIncomeDiscountDetail.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
