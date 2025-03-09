using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-period")]
    public class ReadingPeriodCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodCreateHandler _readingPeriodCreateHandler;
        public ReadingPeriodCreateController(
            IUnitOfWork uow,
            IReadingPeriodCreateHandler readingPeriodCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodCreateHandler = readingPeriodCreateHandler;
            _readingPeriodCreateHandler.NotNull(nameof(readingPeriodCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingPeriodCreateDto createDto, CancellationToken cancellationToken)
        {
            await _readingPeriodCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
