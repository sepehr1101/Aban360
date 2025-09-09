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
    [Route("v1/malfunction-meter")]
    public class MalfunctionMeterController : BaseController
    {
        private readonly IMalfunctionMeterHandler _malfunctionMeterHandler;
        private readonly IReportGenerator _reportGenerator;
        public MalfunctionMeterController(
            IMalfunctionMeterHandler malfunctionMeterHandler,
            IReportGenerator reportGenerator)
        {
            _malfunctionMeterHandler = malfunctionMeterHandler;
            _malfunctionMeterHandler.NotNull(nameof(malfunctionMeterHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto> result = await _malfunctionMeterHandler.Handle(input,cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MalfunctionMeterInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _malfunctionMeterHandler.Handle, CurrentUser, ReportLiterals.MalfunctionMeterSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
