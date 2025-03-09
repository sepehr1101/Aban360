using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-config-default")]
    public class ReadingConfigDefaultDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingConfigDefaultDeleteHandler _readingConfigDefaultDeleteHandler;
        public ReadingConfigDefaultDeleteController(
            IUnitOfWork uow,
            IReadingConfigDefaultDeleteHandler readingConfigDefaultDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingConfigDefaultDeleteHandler = readingConfigDefaultDeleteHandler;
            _readingConfigDefaultDeleteHandler.NotNull(nameof(readingConfigDefaultDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingConfigDefaultDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
