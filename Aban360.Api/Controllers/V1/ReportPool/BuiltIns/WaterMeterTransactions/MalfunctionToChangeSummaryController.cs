using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/malfunction-to-change-summary")]
    public class MalfunctionToChangeSummaryController : BaseController
    {
        private readonly IMalfunctionToChangeSummaryHandler _malfunctionToChangeHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionToChangeSummaryController(
            IMalfunctionToChangeSummaryHandler malfunctionToChangeHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionToChangeHandler = malfunctionToChangeHandler;
            _malfunctionToChangeHandler.NotNull(nameof(malfunctionToChangeHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionToChangeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeSummaryDataOutputDto> result = await _malfunctionToChangeHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionToChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionToChangeHandler.Handle, CurrentUser, ReportLiterals.MalfunctionToChangeSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
