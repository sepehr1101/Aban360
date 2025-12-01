using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class CheckedListGetController : BaseController
    {
        private readonly ICheckedListGetHandler _checkedListGetHandler;
        public CheckedListGetController(ICheckedListGetHandler checkedListGetHandler)
        {
            _checkedListGetHandler = checkedListGetHandler;
            _checkedListGetHandler.NotNull(nameof(checkedListGetHandler));
        }

        [HttpGet,HttpPost]
        [Route("checked-list-get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckedListGet(int id, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto> result = await _checkedListGetHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
