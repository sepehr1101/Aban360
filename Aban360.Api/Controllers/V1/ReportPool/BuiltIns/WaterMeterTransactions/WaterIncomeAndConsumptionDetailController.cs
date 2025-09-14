using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-income-and-consumption-detail")]
    public class WaterIncomeAndConsumptionDetailController : BaseController
    {
        private readonly IWaterIncomeAndConsumptionDetailHandler _waterIncomeAndConsumptionDetail;
        private readonly IReportGenerator _reportGenerator;
        public WaterIncomeAndConsumptionDetailController(
            IWaterIncomeAndConsumptionDetailHandler waterIncomeAndConsumptionDetail,
            IReportGenerator reportGenerator)
        {
            _waterIncomeAndConsumptionDetail = waterIncomeAndConsumptionDetail;
            _waterIncomeAndConsumptionDetail.NotNull(nameof(_waterIncomeAndConsumptionDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterIncomeAndConsumptionDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto> waterIncomeAndConsumption = await _waterIncomeAndConsumptionDetail.Handle(inputDto, cancellationToken);
            return Ok(waterIncomeAndConsumption);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterIncomeAndConsumptionDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterIncomeAndConsumptionDetail.Handle, CurrentUser, ReportLiterals.WaterIncomeAndConsumptionDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WaterIncomeAndConsumptionDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 220;
            ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto> calculationDetails = await _waterIncomeAndConsumptionDetail.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
