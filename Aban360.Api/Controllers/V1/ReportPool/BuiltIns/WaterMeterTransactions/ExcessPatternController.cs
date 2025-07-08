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
        private readonly IExcessPatternHandler _excessPattern;
        public ExcessPatternController(IExcessPatternHandler ExcessPattern)
        {
            _excessPattern = ExcessPattern;
            _excessPattern.NotNull(nameof(_excessPattern));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ExcessPatternInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto> debtorByDay = await _excessPattern.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }
    }
}
