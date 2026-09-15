using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/sms-state-group")]
    public class SmsStateGroupController : BaseController
    {
        private readonly ISmsStateGroupInsertHandler _smsStateGroupInsertHandler;
        private readonly ISmsStateGroupGetAllHandler _smsStateGroupGetAllHandler;
        public SmsStateGroupController(
            ISmsStateGroupInsertHandler smsStateGroupInsertHandler,
            ISmsStateGroupGetAllHandler smsStateGroupGetAllHandler)
        {
            _smsStateGroupInsertHandler = smsStateGroupInsertHandler;
            _smsStateGroupInsertHandler.NotNull(nameof(smsStateGroupInsertHandler));

            _smsStateGroupGetAllHandler = smsStateGroupGetAllHandler;
            _smsStateGroupGetAllHandler.NotNull(nameof(smsStateGroupInsertHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert(SearchInput inputDto, CancellationToken cancellationToken)
        {
            await _smsStateGroupInsertHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(inputDto);
        }
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _smsStateGroupGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
