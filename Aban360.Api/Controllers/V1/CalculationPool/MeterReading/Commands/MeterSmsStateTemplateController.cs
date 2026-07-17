using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-sms-state-template")]
    public class MeterSmsStateTemplateController : BaseController
    {
        private readonly IMeterSmsStateTemplateCreateHandler _meterSmsStateCreateHandler;
        private readonly IMeterSmsStateTemplateRemoveHandler _meterSmsStateRemoveHandler;
        private readonly IMeterSmsStateTemplateGetAllHandler _meterSmsStateGetAllHandler;
        private readonly IMeterSmsStateTemplateGetHandler _meterSmsStateGetHandler;
        public MeterSmsStateTemplateController(
            IMeterSmsStateTemplateCreateHandler meterSmsStateCreateHandler,
            IMeterSmsStateTemplateRemoveHandler meterSmsStateRemoveHandler,
            IMeterSmsStateTemplateGetAllHandler meterSmsStateGetAllHandler,
            IMeterSmsStateTemplateGetHandler meterSmsStateGetHandler)
        {
            _meterSmsStateCreateHandler = meterSmsStateCreateHandler;
            _meterSmsStateCreateHandler.NotNull(nameof(meterSmsStateCreateHandler));

            _meterSmsStateRemoveHandler = meterSmsStateRemoveHandler;
            _meterSmsStateRemoveHandler.NotNull(nameof(meterSmsStateRemoveHandler));

            _meterSmsStateGetAllHandler = meterSmsStateGetAllHandler;
            _meterSmsStateGetAllHandler.NotNull(nameof(meterSmsStateGetAllHandler));

            _meterSmsStateGetHandler = meterSmsStateGetHandler;
            _meterSmsStateGetHandler.NotNull(nameof(meterSmsStateGetHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterSmsStateTemplateInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] MeterSmsStateTemplateInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _meterSmsStateCreateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<short>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _meterSmsStateRemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterSmsStateTemplateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<MeterSmsStateTemplateGetDto> result = await _meterSmsStateGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterSmsStateTemplateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            MeterSmsStateTemplateGetDto result = await _meterSmsStateGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
