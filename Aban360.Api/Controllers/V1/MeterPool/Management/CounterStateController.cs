using Aban360.MeterPool.Application.Features.Management.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using oldCalcPool = Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.MeterPool.Management
{
    public class CounterStateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICounterStateService _service;
        private readonly oldCalcPool.ICounterStateGetAllHandler _c0_oldCalcPoolCounterStateGetAllHandler;
        private readonly ICounterStateGetHandler _c1_counterStateGetHandler;
        public CounterStateController(IUnitOfWork uow, ICounterStateService service, oldCalcPool.ICounterStateGetAllHandler c0_oldCalcPoolCounterStateGetAllHandler, ICounterStateGetHandler c1_counterStateGetHandler)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            c0_oldCalcPoolCounterStateGetAllHandler.NotNull(nameof(c0_oldCalcPoolCounterStateGetAllHandler));
            _c0_oldCalcPoolCounterStateGetAllHandler = c0_oldCalcPoolCounterStateGetAllHandler;
            c1_counterStateGetHandler.NotNull(nameof(c1_counterStateGetHandler));
            _c1_counterStateGetHandler = c1_counterStateGetHandler;
        }

        [HttpPost]
        [Route("~/v1/counter-state/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CounterStateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/counter-state/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CounterStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/v1/counter-state/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CounterStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/counter-state/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CounterStateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CounterStateGetDto counterStates = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(counterStates);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/counter-state/all-aban360")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CounterStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAban360(CancellationToken cancellationToken)
        {
            ICollection<CounterStateGetDto> counterStates = await _service.GetAllAsync(cancellationToken);
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
