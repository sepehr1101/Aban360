using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/reading-period-type")]
    public class ReadingPeriodTypeGetAllController : BaseController
    {
        private readonly IReadingPeriodTypeGetAllHandler _readingPeriodTypeGetAllHandler;
        public ReadingPeriodTypeGetAllController(IReadingPeriodTypeGetAllHandler readingPeriodTypeGetAllHandler)
        {
            _readingPeriodTypeGetAllHandler = readingPeriodTypeGetAllHandler;
            _readingPeriodTypeGetAllHandler.NotNull(nameof(readingPeriodTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingPeriodTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var readingPeriodTypes = await _readingPeriodTypeGetAllHandler.Handle(cancellationToken);
            return Ok(readingPeriodTypes);
        }
    }
}
