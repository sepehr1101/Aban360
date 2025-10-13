using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterToChangeTransactions
{
    [Route("v1/malfunction-to-change-detail")]
    public class MalfunctionToChangeDetailController : BaseController
    {
        private readonly IMalfunctionToChangeDetailHandler _malfunctionToChangeHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionToChangeDetailController(
            IMalfunctionToChangeDetailHandler malfunctionToChangeHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionToChangeHandler = malfunctionToChangeHandler;
            _malfunctionToChangeHandler.NotNull(nameof(malfunctionToChangeHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionToChangeInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto> result = await _malfunctionToChangeHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionToChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionToChangeHandler.Handle, CurrentUser, ReportLiterals.MalfunctionToChangeDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(MalfunctionToChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 370;
            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto> result = await _malfunctionToChangeHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}