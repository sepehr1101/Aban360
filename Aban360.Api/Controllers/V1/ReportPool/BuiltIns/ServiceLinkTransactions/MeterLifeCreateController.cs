using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/meter-life")]
    public class MeterLifeCreateController : BaseController
    {
        private readonly IMeterLifeInsertHandler _meterLifeGetHandler;
        public MeterLifeCreateController(IMeterLifeInsertHandler meterLifeGetHandler)
        {
            _meterLifeGetHandler = meterLifeGetHandler;
            _meterLifeGetHandler.NotNull(nameof(meterLifeGetHandler));
        }

        [HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await _meterLifeGetHandler.Handle(cancellationToken);
            return Ok();
        }
    }
}
