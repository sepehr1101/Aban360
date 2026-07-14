using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-sms-flow")]
    public class MeterSmsFlowController : BaseController
    {
        private readonly IMeterSmsFlowCreateHandler _meterSmsStateCreateHandler;
        private readonly IMeterSmsFlowUpdateHandler _meterSmsStateUpdateHandler;
        private readonly IMeterSmsFlowGetAllHandler _meterSmsStateGetAllHandler;
        private readonly IMeterSmsFlowGetHandler _meterSmsStateGetHandler;
        public MeterSmsFlowController(
            IMeterSmsFlowCreateHandler meterSmsStateCreateHandler,
            IMeterSmsFlowUpdateHandler meterSmsStateUpdateHandler,
            IMeterSmsFlowGetAllHandler meterSmsStateGetAllHandler,
            IMeterSmsFlowGetHandler meterSmsStateGetHandler)
        {
            _meterSmsStateCreateHandler = meterSmsStateCreateHandler;
            _meterSmsStateCreateHandler.NotNull(nameof(meterSmsStateCreateHandler));

            _meterSmsStateUpdateHandler = meterSmsStateUpdateHandler;
            _meterSmsStateUpdateHandler.NotNull(nameof(meterSmsStateUpdateHandler));

            _meterSmsStateGetAllHandler = meterSmsStateGetAllHandler;
            _meterSmsStateGetAllHandler.NotNull(nameof(meterSmsStateGetAllHandler));

            _meterSmsStateGetHandler = meterSmsStateGetHandler;
            _meterSmsStateGetHandler.NotNull(nameof(meterSmsStateGetHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterSmsFlowInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] MeterSmsFlowInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _meterSmsStateCreateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterSmsFlowUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterSmsFlowUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _meterSmsStateUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterSmsFlowGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<MeterSmsFlowGetDto> result = await _meterSmsStateGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterSmsFlowGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            MeterSmsFlowGetDto result = await _meterSmsStateGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
