using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/excess-pattern")]
    public class ExcessPatternController : BaseController
    {
        private readonly IExcessPatternHandler _ExcessPattern;
        private readonly IReportGenerator _reportGenerator;
        public ExcessPatternController(
            IExcessPatternHandler ExcessPattern,
            IReportGenerator reportGenerator)
        {
            _ExcessPattern = ExcessPattern;
            _ExcessPattern.NotNull(nameof(_ExcessPattern));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ExcessPatternInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto> debtorByDay = await _ExcessPattern.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ExcessPatternInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _ExcessPattern.Handle, CurrentUser, ReportLiterals.ExcessPattern, connectionId);
            return Ok(inputDto);
        }
    }
}
