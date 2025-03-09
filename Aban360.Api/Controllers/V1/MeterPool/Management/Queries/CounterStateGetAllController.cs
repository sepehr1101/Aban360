using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/counter-state")]
    public class CounterStateGetAllController : BaseController
    {
        private readonly ICounterStateGetAllHandler _counterStateGetAllHandler;
        public CounterStateGetAllController(ICounterStateGetAllHandler counterStateGetAllHandler)
        {
            _counterStateGetAllHandler = counterStateGetAllHandler;
            _counterStateGetAllHandler.NotNull(nameof(counterStateGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CounterStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var counterStates = await _counterStateGetAllHandler.Handle(cancellationToken);
            return Ok(counterStates);
        }
    }
}
