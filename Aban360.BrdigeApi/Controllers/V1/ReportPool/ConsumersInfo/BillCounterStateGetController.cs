using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillCounterStateGetController : BaseController
    {
        private readonly ICounterStateGetHandler _counterStateGetHandler;
        public BillCounterStateGetController(ICounterStateGetHandler counterStateGetHandler)
        {
            _counterStateGetHandler = counterStateGetHandler;
            _counterStateGetHandler.NotNull(nameof(counterStateGetHandler));
        }

        [HttpGet]
        [Route("counter-state")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<StringDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<StringDictionary> result = await _counterStateGetHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
