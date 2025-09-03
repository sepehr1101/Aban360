using Aban360.Api.Cronjobs;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-income-and-consumption-summary")]
    public class WaterIncomeAndConsumptionSummaryController : BaseController
    {
        private readonly IWaterIncomeAndConsumptionSummaryHandler _waterIncomeAndConsumptionSummary;
        private readonly IReportGenerator _reportGenerator;
        public WaterIncomeAndConsumptionSummaryController(
            IWaterIncomeAndConsumptionSummaryHandler waterIncomeAndConsumptionSummary,
            IReportGenerator reportGenerator)
        {
            _waterIncomeAndConsumptionSummary = waterIncomeAndConsumptionSummary;
            _waterIncomeAndConsumptionSummary.NotNull(nameof(_waterIncomeAndConsumptionSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterIncomeAndConsumptionSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto> waterIncomeAndConsumption = await _waterIncomeAndConsumptionSummary.Handle(inputDto, cancellationToken);
            return Ok(waterIncomeAndConsumption);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterIncomeAndConsumptionSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportTitle = GetReportTitle(inputDto.EnumInput);
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterIncomeAndConsumptionSummary.Handle, CurrentUser, ReportLiterals.WaterIncomeAndConsumptionSummary + reportTitle, connectionId);
            return Ok(inputDto);
        }
        private string GetReportTitle(WaterIncomeAndConsumptionSummaryEnum enumState)
        {
            string baseReportTitle = " بر اساس ";
            Dictionary<int, string> titles = new Dictionary<int, string>()
            {
                { 0,"کاربری"},
                { 1,"ناحیه"},
                { 2,"روز"},
                { 3,"میانگین مصرف"},
            };
            return baseReportTitle + titles[(int)enumState];
        }
    }
}
