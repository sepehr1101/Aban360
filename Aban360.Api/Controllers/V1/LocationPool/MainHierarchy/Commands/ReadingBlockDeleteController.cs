using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/reading-block")]
    public class ReadingBlocKDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBlockDeleteHandler _readingBlockDeleteHandler;
        public ReadingBlocKDeleteController(
            IUnitOfWork uow,
            IReadingBlockDeleteHandler readingBlockDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBlockDeleteHandler = readingBlockDeleteHandler;
            _readingBlockDeleteHandler.NotNull(nameof(readingBlockDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] ReadingBlockDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingBlockDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
