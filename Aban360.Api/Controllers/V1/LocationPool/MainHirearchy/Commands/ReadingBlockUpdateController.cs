using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/reading-block")]
    public class ReadingBlocKUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBlockUpdateHandler _readingBlockUpdateHandler;
        public ReadingBlocKUpdateController(
            IUnitOfWork uow,
            IReadingBlockUpdateHandler readingBlockUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBlockUpdateHandler = readingBlockUpdateHandler;
            _readingBlockUpdateHandler.NotNull(nameof(readingBlockUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] ReadingBlockUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingBlockUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
