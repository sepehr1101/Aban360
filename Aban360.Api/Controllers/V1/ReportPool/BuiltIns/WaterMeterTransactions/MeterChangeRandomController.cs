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
    [Route("v1/meter-change-random")]
    public class MeterChangeRandomController : BaseController
    {
        private readonly IMeterChangeRandomHandler _meterChangeRandom;
        private readonly IReportGenerator _reportGenerator;
        public MeterChangeRandomController(
            IMeterChangeRandomHandler MeterChangeRandom,
            IReportGenerator reportGenerator)
        {
            _meterChangeRandom = MeterChangeRandom;
            _meterChangeRandom.NotNull(nameof(_meterChangeRandom));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterChangeRandomHeaderOutputDto, MeterChangeRandomDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(MeterChangeRandomInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterChangeRandomHeaderOutputDto, MeterChangeRandomDataOutputDto> MeterChangeRandom = await _meterChangeRandom.Handle(inputDto, cancellationToken);
            return Ok(MeterChangeRandom);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, MeterChangeRandomInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _meterChangeRandom.Handle, CurrentUser, ReportLiterals.MeterChangeRandom, connectionId);
            return Ok(inputDto);
        }
    }
}
