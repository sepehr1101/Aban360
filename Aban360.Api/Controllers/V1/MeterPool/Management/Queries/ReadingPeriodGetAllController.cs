using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Queries
{
    [Route("v1/reading-period")]
    public class ReadingPeriodGetAllController : BaseController
    {
        private readonly IReadingPeriodGetAllHandler _readingPeriodGetAllHandler;
        public ReadingPeriodGetAllController(IReadingPeriodGetAllHandler readingPeriodGetAllHandler)
        {
            _readingPeriodGetAllHandler = readingPeriodGetAllHandler;
            _readingPeriodGetAllHandler.NotNull(nameof(readingPeriodGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingPeriodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingPeriodGetDto> readingPeriods = await _readingPeriodGetAllHandler.Handle(cancellationToken);
            return Ok(readingPeriods);
        }
    }
}
