using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/counter-state")]
    public class CounterStateGetSingleController : BaseController
    {
        private readonly ICounterStateGetSingleHandler _counterStateGetSingleHandler;
        public CounterStateGetSingleController(ICounterStateGetSingleHandler counterStateGetSingleHandler)
        {
            _counterStateGetSingleHandler = counterStateGetSingleHandler;
            _counterStateGetSingleHandler.NotNull(nameof(counterStateGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CounterStateGetDto counterStates = await _counterStateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(counterStates);
        }
    }
}
