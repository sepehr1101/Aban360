using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/water-meter-replacements")]
    public class WaterMeterReplacementsController : BaseController
    {
        private readonly IWaterMeterReplacementsHandler _waterMeterReplacementsHandler;
        private readonly IReportGenerator _reportGenerator;
        public WaterMeterReplacementsController(
            IWaterMeterReplacementsHandler waterMeterReplacementsHandler,
            IReportGenerator reportGenerator)
        {
            _waterMeterReplacementsHandler = waterMeterReplacementsHandler;
            _waterMeterReplacementsHandler.NotNull(nameof(waterMeterReplacementsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterMeterReplacementsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto> waterMeterReplacements = await _waterMeterReplacementsHandler.Handle(input, cancellationToken);
            return Ok(waterMeterReplacements);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WaterMeterReplacementsInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsChangeDate ? ReportLiterals.ChangeDate : ReportLiterals.RegisterDate;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterMeterReplacementsHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
