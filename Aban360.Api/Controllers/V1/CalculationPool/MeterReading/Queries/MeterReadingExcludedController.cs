using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Queries
{
    [Route("v1/meter-flow")]
    public class MeterReadingExcludedController : BaseController
    {
        private readonly IMeterReadingDetailExcludedGetHadler _excludedGetHandler;
        public MeterReadingExcludedController(IMeterReadingDetailExcludedGetHadler excludedGetHandler)
        {
            _excludedGetHandler = excludedGetHandler;
            _excludedGetHandler.NotNull(nameof(excludedGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("excluded")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutptuDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcluded([FromBody]MeterReadingDetailExcludedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutptuDto> result = await _excludedGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
