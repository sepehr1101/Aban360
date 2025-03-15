using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/reading-block")]
    public class ReadingBlocKCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBlockCreateHandler _readingBlockCreateHandler;
        public ReadingBlocKCreateController(
            IUnitOfWork uow,
            IReadingBlockCreateHandler readingBlockCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBlockCreateHandler = readingBlockCreateHandler;
            _readingBlockCreateHandler.NotNull(nameof(readingBlockCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBlockCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingBlockCreateDto createDto, CancellationToken cancellationToken)
        {
            await _readingBlockCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
