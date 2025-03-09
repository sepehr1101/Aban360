using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-config-default")]
    public class ReadingConfigDefaultUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingConfigDefaultUpdateHandler _readingConfigDefaultUpdateHandler;
        public ReadingConfigDefaultUpdateController(
            IUnitOfWork uow,
            IReadingConfigDefaultUpdateHandler readingConfigDefaultUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingConfigDefaultUpdateHandler = readingConfigDefaultUpdateHandler;
            _readingConfigDefaultUpdateHandler.NotNull(nameof(readingConfigDefaultUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] ReadingConfigDefaultUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingConfigDefaultUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
