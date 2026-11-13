using Aban360.Api.Controllers.V1;
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
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.Api.Filters;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.MeterPool.Management
{
    public class CounterStateController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ICounterStateCreateHandler _c0_counterStateCreateHandler;
        private readonly ICounterStateUpdateHandler _c0_counterStateUpdateHandler;
        private readonly ICounterStateDeleteHandler _c0_counterStateDeleteHandler;
        private readonly ICounterStateGetSingleHandler _c0_counterStateGetSingleHandler;
        private readonly meterPool.ICounterStateGetAllHandler _c0_meterPoolCounterStateGetAllHandler;
        private readonly oldCalcPool.ICounterStateGetAllHandler _c0_oldCalcPoolCounterStateGetAllHandler;
        private readonly ICounterStateGetHandler _c1_counterStateGetHandler;
        public CounterStateController(IUnitOfWork c0_uow, ICounterStateCreateHandler c0_counterStateCreateHandler, ICounterStateUpdateHandler c0_counterStateUpdateHandler, ICounterStateDeleteHandler c0_counterStateDeleteHandler, ICounterStateGetSingleHandler c0_counterStateGetSingleHandler, meterPool.ICounterStateGetAllHandler c0_meterPoolCounterStateGetAllHandler, oldCalcPool.ICounterStateGetAllHandler c0_oldCalcPoolCounterStateGetAllHandler, ICounterStateGetHandler c1_counterStateGetHandler)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c0_counterStateCreateHandler = c0_counterStateCreateHandler;
            _c0_counterStateCreateHandler.NotNull(nameof(c0_counterStateCreateHandler));
            _c0_counterStateUpdateHandler = c0_counterStateUpdateHandler;
            _c0_counterStateUpdateHandler.NotNull(nameof(c0_counterStateUpdateHandler));
            _c0_counterStateDeleteHandler = c0_counterStateDeleteHandler;
            _c0_counterStateDeleteHandler.NotNull(nameof(c0_counterStateDeleteHandler));
            _c0_counterStateGetSingleHandler = c0_counterStateGetSingleHandler;
            _c0_counterStateGetSingleHandler.NotNull(nameof(c0_counterStateGetSingleHandler));
            _c0_meterPoolCounterStateGetAllHandler = c0_meterPoolCounterStateGetAllHandler;
            _c0_meterPoolCounterStateGetAllHandler.NotNull(nameof(c0_meterPoolCounterStateGetAllHandler));
            _c0_oldCalcPoolCounterStateGetAllHandler = c0_oldCalcPoolCounterStateGetAllHandler;
            _c0_oldCalcPoolCounterStateGetAllHandler.NotNull(nameof(c0_oldCalcPoolCounterStateGetAllHandler));
            _c1_counterStateGetHandler = c1_counterStateGetHandler;
            _c1_counterStateGetHandler.NotNull(nameof(c1_counterStateGetHandler));
        }

        [HttpPost]
        [Route("~/v1/counter-state/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CounterStateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _c0_counterStateCreateHandler.Handle(createDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/counter-state/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CounterStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c0_counterStateUpdateHandler.Handle(updateDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/v1/counter-state/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CounterStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _c0_counterStateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/counter-state/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CounterStateGetDto counterStates = await _c0_counterStateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(counterStates);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/counter-state/all-aban360")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CounterStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAban360(CancellationToken cancellationToken)
        {
            ICollection<CounterStateGetDto> counterStates = await _c0_meterPoolCounterStateGetAllHandler.Handle(cancellationToken);
            return Ok(counterStates);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/counter-state/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CounterStateCodeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([Optional] bool isReadable, CancellationToken cancellationToken)
        {
            IEnumerable<CounterStateCodeDto> counterStates = await _c0_oldCalcPoolCounterStateGetAllHandler.Handle(isReadable, cancellationToken);
            return Ok(counterStates);
        }

        [HttpGet]
        [Route("~/v1/counter-state/get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<StringDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<StringDictionary> result = await _c1_counterStateGetHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
