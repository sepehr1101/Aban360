using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-config-default")]
    public class ReadingConfigDefaultCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingConfigDefaultCreateHandler _readingConfigDefaultCreateHandler;
        public ReadingConfigDefaultCreateController(
            IUnitOfWork uow,
            IReadingConfigDefaultCreateHandler readingConfigDefaultCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingConfigDefaultCreateHandler = readingConfigDefaultCreateHandler;
            _readingConfigDefaultCreateHandler.NotNull(nameof(readingConfigDefaultCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingConfigDefaultCreateDto createDto, CancellationToken cancellationToken)
        {
            await _readingConfigDefaultCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
