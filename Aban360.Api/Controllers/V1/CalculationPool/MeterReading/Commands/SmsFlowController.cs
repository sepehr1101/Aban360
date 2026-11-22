using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/sms-flow")]
    public class SmsFlowController : BaseController
    {
        private readonly ISmsFlowService _smsFlowService;
        public SmsFlowController(ISmsFlowService smsFlowService)
        {
            _smsFlowService = smsFlowService;
            _smsFlowService.NotNull(nameof(smsFlowService));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SmsFlowInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] SmsFlowInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _smsFlowService.CreateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SmsFlowUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SmsFlowUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _smsFlowService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SmsFlowGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<SmsFlowGetDto> result = await _smsFlowService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SmsFlowGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            SmsFlowGetDto result = await _smsFlowService.GetSingleAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
