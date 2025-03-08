using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-period")]
    public class ReadingPeriodDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodDeleteHandler _readingPeriodDeleteHandler;
        public ReadingPeriodDeleteController(
            IUnitOfWork uow,
            IReadingPeriodDeleteHandler readingPeriodDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodDeleteHandler = readingPeriodDeleteHandler;
            _readingPeriodDeleteHandler.NotNull(nameof(readingPeriodDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingPeriodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingPeriodDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
