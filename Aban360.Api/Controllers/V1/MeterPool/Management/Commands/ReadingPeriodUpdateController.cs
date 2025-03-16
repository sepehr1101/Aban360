using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-period")]
    public class ReadingPeriodUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodUpdateHandler _readingPeriodUpdateHandler;
        public ReadingPeriodUpdateController(
            IUnitOfWork uow,
            IReadingPeriodUpdateHandler readingPeriodUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodUpdateHandler = readingPeriodUpdateHandler;
            _readingPeriodUpdateHandler.NotNull(nameof(readingPeriodUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingPeriodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingPeriodUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
