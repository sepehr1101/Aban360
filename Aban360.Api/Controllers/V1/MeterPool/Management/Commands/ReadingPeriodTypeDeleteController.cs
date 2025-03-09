using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-period-type")]
    public class ReadingPeriodTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodTypeDeleteHandler _readingPeriodTypeDeleteHandler;
        public ReadingPeriodTypeDeleteController(
            IUnitOfWork uow,
            IReadingPeriodTypeDeleteHandler readingPeriodTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodTypeDeleteHandler = readingPeriodTypeDeleteHandler;
            _readingPeriodTypeDeleteHandler.NotNull(nameof(readingPeriodTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingPeriodTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingPeriodTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
