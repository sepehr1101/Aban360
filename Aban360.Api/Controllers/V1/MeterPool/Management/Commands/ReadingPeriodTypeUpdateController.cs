using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-period-type")]
    public class ReadingPeriodTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodTypeUpdateHandler _readingPeriodTypeUpdateHandler;
        public ReadingPeriodTypeUpdateController(
            IUnitOfWork uow,
            IReadingPeriodTypeUpdateHandler readingPeriodTypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodTypeUpdateHandler = readingPeriodTypeUpdateHandler;
            _readingPeriodTypeUpdateHandler.NotNull(nameof(readingPeriodTypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingPeriodTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingPeriodTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
