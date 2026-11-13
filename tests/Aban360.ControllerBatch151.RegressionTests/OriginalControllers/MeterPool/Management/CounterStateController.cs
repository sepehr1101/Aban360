using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using meterPool = Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using oldCalcPool = Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;

namespace Aban360.Api.Controllers.V1.BeforeBatch151.MeterPool.Management
{
    [Route("v1/counter-state")]
    public class CounterStateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICounterStateCreateHandler _counterStateCreateHandler;
        private readonly ICounterStateUpdateHandler _counterStateUpdateHandler;
        private readonly ICounterStateDeleteHandler _counterStateDeleteHandler;
        private readonly ICounterStateGetSingleHandler _counterStateGetSingleHandler;
        private readonly meterPool.ICounterStateGetAllHandler _meterPoolCounterStateGetAllHandler;
        private readonly oldCalcPool.ICounterStateGetAllHandler _oldCalcPoolCounterStateGetAllHandler;

        public CounterStateController(
            IUnitOfWork uow,
            ICounterStateCreateHandler counterStateCreateHandler,
            ICounterStateUpdateHandler counterStateUpdateHandler,
            ICounterStateDeleteHandler counterStateDeleteHandler,
            ICounterStateGetSingleHandler counterStateGetSingleHandler,
            meterPool.ICounterStateGetAllHandler meterPoolCounterStateGetAllHandler,
            oldCalcPool.ICounterStateGetAllHandler oldCalcPoolCounterStateGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _counterStateCreateHandler = counterStateCreateHandler;
            _counterStateCreateHandler.NotNull(nameof(counterStateCreateHandler));

            _counterStateUpdateHandler = counterStateUpdateHandler;
            _counterStateUpdateHandler.NotNull(nameof(counterStateUpdateHandler));

            _counterStateDeleteHandler = counterStateDeleteHandler;
            _counterStateDeleteHandler.NotNull(nameof(counterStateDeleteHandler));

            _counterStateGetSingleHandler = counterStateGetSingleHandler;
            _counterStateGetSingleHandler.NotNull(nameof(counterStateGetSingleHandler));

            _meterPoolCounterStateGetAllHandler = meterPoolCounterStateGetAllHandler;
            _meterPoolCounterStateGetAllHandler.NotNull(nameof(meterPoolCounterStateGetAllHandler));

            _oldCalcPoolCounterStateGetAllHandler = oldCalcPoolCounterStateGetAllHandler;
            _oldCalcPoolCounterStateGetAllHandler.NotNull(nameof(oldCalcPoolCounterStateGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CounterStateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _counterStateCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CounterStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _counterStateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CounterStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _counterStateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CounterStateGetDto counterStates = await _counterStateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(counterStates);
        }

        [HttpPost, HttpGet]
        [Route("all-aban360")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CounterStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAban360(CancellationToken cancellationToken)
        {
            ICollection<CounterStateGetDto> counterStates = await _meterPoolCounterStateGetAllHandler.Handle(cancellationToken);
            return Ok(counterStates);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CounterStateCodeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([Optional] bool isReadable, CancellationToken cancellationToken)
        {
            IEnumerable<CounterStateCodeDto> counterStates = await _oldCalcPoolCounterStateGetAllHandler.Handle(isReadable, cancellationToken);
            return Ok(counterStates);
        }
    }
}
