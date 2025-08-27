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
    [Route("v1/water-net-income")]
    public class WaterNetIncomeController : BaseController
    {
        private readonly IWaterNetIncomeHandler _waterNetIncomeHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterNetIncomeController(
            IWaterNetIncomeHandler waterNetIncomeHandler,
            IReportGenerator reportGenerator)
        {
            _waterNetIncomeHandler = waterNetIncomeHandler;
            _waterNetIncomeHandler.NotNull(nameof(waterNetIncomeHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterNetIncomeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto> WaterNetIncome = await _waterNetIncomeHandler.Handle(input, cancellationToken);
            return Ok(WaterNetIncome);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterNetIncomeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterNetIncomeHandler.Handle, CurrentUser, ReportLiterals.WaterNetIncome, connectionId);
            return Ok(inputDto);
        }
    }
}
