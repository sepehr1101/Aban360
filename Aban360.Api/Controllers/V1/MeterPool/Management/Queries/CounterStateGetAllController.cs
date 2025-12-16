using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using meterPool = Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using oldCalcPool = Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/counter-state")]
    public class CounterStateGetAllController : BaseController
    {
        private readonly meterPool.ICounterStateGetAllHandler _counterStateGetAllMeterPoolHandler;
        private readonly oldCalcPool.ICounterStateGetAllHandler _counterStateGetAllOldCalcPoolHandler;
        public CounterStateGetAllController(
            meterPool.ICounterStateGetAllHandler counterStateGetAllMeterPoolHandler,
            oldCalcPool.ICounterStateGetAllHandler counterStateGetAllOldCalcPoolHandler)
        {
            _counterStateGetAllMeterPoolHandler = counterStateGetAllMeterPoolHandler;
            _counterStateGetAllMeterPoolHandler.NotNull(nameof(counterStateGetAllMeterPoolHandler));

            _counterStateGetAllOldCalcPoolHandler = counterStateGetAllOldCalcPoolHandler;
            _counterStateGetAllOldCalcPoolHandler.NotNull(nameof(counterStateGetAllOldCalcPoolHandler));
        }

        [HttpPost, HttpGet]
        [Route("all-aban360")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CounterStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAban360(CancellationToken cancellationToken)
        {
            ICollection<CounterStateGetDto> counterStates = await _counterStateGetAllMeterPoolHandler.Handle(cancellationToken);
            return Ok(counterStates);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CounterStateCodeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<CounterStateCodeDto> counterStates = await _counterStateGetAllOldCalcPoolHandler.Handle(cancellationToken);
            return Ok(counterStates);
        }
    }
}
