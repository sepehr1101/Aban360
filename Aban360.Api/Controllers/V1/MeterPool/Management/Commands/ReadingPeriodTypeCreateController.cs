using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management.Commands
{
    [Route("v1/reading-period-type")]
    public class ReadingPeriodTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodTypeCreateHandler _readingPeriodTypeCreateHandler;
        public ReadingPeriodTypeCreateController(
            IUnitOfWork uow,
            IReadingPeriodTypeCreateHandler readingPeriodTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodTypeCreateHandler = readingPeriodTypeCreateHandler;
            _readingPeriodTypeCreateHandler.NotNull(nameof(readingPeriodTypeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingPeriodTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _readingPeriodTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
