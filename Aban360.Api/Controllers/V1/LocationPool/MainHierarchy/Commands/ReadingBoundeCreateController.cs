using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBoundCreateHandler _readingBoundCreateHandler;
        public ReadingBoundeCreateController(
            IUnitOfWork uow,
            IReadingBoundCreateHandler readingBoundCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBoundCreateHandler = readingBoundCreateHandler;
            _readingBoundCreateHandler.NotNull(nameof(readingBoundCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ReadingBoundCreateDto createDto, CancellationToken cancellationToken)
        {
            await _readingBoundCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
