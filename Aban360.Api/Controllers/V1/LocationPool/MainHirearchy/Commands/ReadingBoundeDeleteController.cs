using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBoundDeleteHandler _readingBoundDeleteHandler;
        public ReadingBoundeDeleteController(
            IUnitOfWork uow,
            IReadingBoundDeleteHandler readingBoundDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBoundDeleteHandler = readingBoundDeleteHandler;
            _readingBoundDeleteHandler.NotNull(nameof(readingBoundDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] ReadingBoundDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingBoundDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
