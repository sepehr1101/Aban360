using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-calculation-details")]
    public class WaterCalculationDetailsController : BaseController
    {
        private readonly IWaterCalculationDetailsHandler _calculationDetailsHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterCalculationDetailsController(
            IWaterCalculationDetailsHandler calculationDetailsHandler,
            IReportGenerator reportGenerator)
        {
            _calculationDetailsHandler = calculationDetailsHandler;
            _calculationDetailsHandler.NotNull(nameof(calculationDetailsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterCalculationDetailsHeaderOutputDto, WaterCalculationDetailsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterCalculationDetailsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterCalculationDetailsHeaderOutputDto, WaterCalculationDetailsDataOutputDto> calculationDetails = await _calculationDetailsHandler.Handle(input, cancellationToken);
            return Ok(calculationDetails);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterCalculationDetailsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _calculationDetailsHandler.Handle, CurrentUser, ReportLiterals.WaterCalculationDetails, connectionId);
            return Ok(inputDto);
        }
    }
}
