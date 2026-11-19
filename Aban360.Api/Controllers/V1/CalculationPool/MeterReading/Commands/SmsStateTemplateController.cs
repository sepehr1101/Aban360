using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/sms-state-template")]
    public class SmsStateTemplateController : BaseController
    {
        private readonly ISmsStateTemplateService _smsStateTemplateService;
        public SmsStateTemplateController(ISmsStateTemplateService smsStateTemplateService)
        {
            _smsStateTemplateService = smsStateTemplateService;
            _smsStateTemplateService.NotNull(nameof(smsStateTemplateService));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SmsStateTemplateInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] SmsStateTemplateInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _smsStateTemplateService.CreateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<short>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _smsStateTemplateService.DeleteAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SmsStateTemplateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<SmsStateTemplateGetDto> result = await _smsStateTemplateService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SmsStateTemplateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            SmsStateTemplateGetDto result = await _smsStateTemplateService.GetSingleAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
