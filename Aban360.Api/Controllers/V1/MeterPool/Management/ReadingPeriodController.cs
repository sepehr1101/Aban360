using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Management
{
    [Route("v1/reading-period")]
    public class ReadingPeriodController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodCreateHandler _readingPeriodCreateHandler;
        private readonly IReadingPeriodUpdateHandler _readingPeriodUpdateHandler;
        private readonly IReadingPeriodDeleteHandler _readingPeriodDeleteHandler;
        private readonly IReadingPeriodGetSingleHandler _readingPeriodGetSingleHandler;
        private readonly IReadingPeriodGetAllHandler _readingPeriodGetAllHandler;

        public ReadingPeriodController(
            IUnitOfWork uow,
            IReadingPeriodCreateHandler readingPeriodCreateHandler,
            IReadingPeriodUpdateHandler readingPeriodUpdateHandler,
            IReadingPeriodDeleteHandler readingPeriodDeleteHandler,
            IReadingPeriodGetSingleHandler readingPeriodGetSingleHandler,
            IReadingPeriodGetAllHandler readingPeriodGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodCreateHandler = readingPeriodCreateHandler;
            _readingPeriodCreateHandler.NotNull(nameof(readingPeriodCreateHandler));

            _readingPeriodUpdateHandler = readingPeriodUpdateHandler;
            _readingPeriodUpdateHandler.NotNull(nameof(readingPeriodUpdateHandler));

            _readingPeriodDeleteHandler = readingPeriodDeleteHandler;
            _readingPeriodDeleteHandler.NotNull(nameof(readingPeriodDeleteHandler));

            _readingPeriodGetSingleHandler = readingPeriodGetSingleHandler;
            _readingPeriodGetSingleHandler.NotNull(nameof(readingPeriodGetSingleHandler));

            _readingPeriodGetAllHandler = readingPeriodGetAllHandler;
            _readingPeriodGetAllHandler.NotNull(nameof(readingPeriodGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingPeriodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingPeriodUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingPeriodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingPeriodDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingPeriodGetDto readingPeriods = await _readingPeriodGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingPeriods);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingPeriodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingPeriodGetDto> readingPeriods = await _readingPeriodGetAllHandler.Handle(cancellationToken);
            return Ok(readingPeriods);
        }
    }
}
