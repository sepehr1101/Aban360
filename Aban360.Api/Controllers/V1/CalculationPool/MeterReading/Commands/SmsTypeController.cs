using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/sms-type")]
    public class SmsTypeController : BaseController
    {
        private readonly ISmsTypeInsertHandler _smsTypeInsertHandler;
        private readonly ISmsTypeGetAllHandler _smsTypeGetAllHandler;
        public SmsTypeController(
            ISmsTypeInsertHandler smsTypeInsertHandler,
            ISmsTypeGetAllHandler smsTypeGetAllHandler)
        {
            _smsTypeInsertHandler = smsTypeInsertHandler;
            _smsTypeInsertHandler.NotNull(nameof(smsTypeInsertHandler));

            _smsTypeGetAllHandler = smsTypeGetAllHandler;
            _smsTypeGetAllHandler.NotNull(nameof(smsTypeInsertHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert(SearchInput inputDto, CancellationToken cancellationToken)
        {
            await _smsTypeInsertHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(inputDto);
        }
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _smsTypeGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

    }
}
