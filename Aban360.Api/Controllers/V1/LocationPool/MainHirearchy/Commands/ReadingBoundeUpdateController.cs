using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBoundUpdateHandler _readingBoundUpdateHandler;
        public ReadingBoundeUpdateController(
            IUnitOfWork uow,
            IReadingBoundUpdateHandler readingBoundUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBoundUpdateHandler = readingBoundUpdateHandler;
            _readingBoundUpdateHandler.NotNull(nameof(readingBoundUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] ReadingBoundUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingBoundUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
