// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
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

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.MeterPool.Management
{
    [Route("v1/reading-period-type")]
    public class ReadingPeriodTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingPeriodTypeCreateHandler _readingPeriodTypeCreateHandler;
        private readonly IReadingPeriodTypeUpdateHandler _readingPeriodTypeUpdateHandler;
        private readonly IReadingPeriodTypeDeleteHandler _readingPeriodTypeDeleteHandler;
        private readonly IReadingPeriodTypeGetSingleHandler _readingPeriodTypeGetSingleHandler;
        private readonly IReadingPeriodTypeGetAllHandler _readingPeriodTypeGetAllHandler;

        public ReadingPeriodTypeController(
            IUnitOfWork uow,
            IReadingPeriodTypeCreateHandler readingPeriodTypeCreateHandler,
            IReadingPeriodTypeUpdateHandler readingPeriodTypeUpdateHandler,
            IReadingPeriodTypeDeleteHandler readingPeriodTypeDeleteHandler,
            IReadingPeriodTypeGetSingleHandler readingPeriodTypeGetSingleHandler,
            IReadingPeriodTypeGetAllHandler readingPeriodTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingPeriodTypeCreateHandler = readingPeriodTypeCreateHandler;
            _readingPeriodTypeCreateHandler.NotNull(nameof(readingPeriodTypeCreateHandler));

            _readingPeriodTypeUpdateHandler = readingPeriodTypeUpdateHandler;
            _readingPeriodTypeUpdateHandler.NotNull(nameof(readingPeriodTypeUpdateHandler));

            _readingPeriodTypeDeleteHandler = readingPeriodTypeDeleteHandler;
            _readingPeriodTypeDeleteHandler.NotNull(nameof(readingPeriodTypeDeleteHandler));

            _readingPeriodTypeGetSingleHandler = readingPeriodTypeGetSingleHandler;
            _readingPeriodTypeGetSingleHandler.NotNull(nameof(readingPeriodTypeGetSingleHandler));

            _readingPeriodTypeGetAllHandler = readingPeriodTypeGetAllHandler;
            _readingPeriodTypeGetAllHandler.NotNull(nameof(readingPeriodTypeGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingPeriodTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingPeriodTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingPeriodTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingPeriodTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingPeriodTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingPeriodTypeGetDto readingPeriodTypes = await _readingPeriodTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingPeriodTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingPeriodTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingPeriodTypeGetDto> readingPeriodTypes = await _readingPeriodTypeGetAllHandler.Handle(cancellationToken);
            return Ok(readingPeriodTypes);
        }
    }
}
