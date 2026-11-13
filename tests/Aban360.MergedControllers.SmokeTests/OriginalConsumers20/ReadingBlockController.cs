// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.LocationPool.MainHierarchy;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/reading-block")]
    public class ReadingBlockController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBlockCreateHandler _readingBlockCreateHandler;
        private readonly IReadingBlockUpdateHandler _readingBlockUpdateHandler;
        private readonly IReadingBlockDeleteHandler _readingBlockDeleteHandler;
        private readonly IReadingBlockGetSingleHandler _readingBlockGetSingleHandler;
        private readonly IReadingBlockGetAllHandler _readingBlockGetAllHandler;

        public ReadingBlockController(
            IUnitOfWork uow,
            IReadingBlockCreateHandler readingBlockCreateHandler,
            IReadingBlockUpdateHandler readingBlockUpdateHandler,
            IReadingBlockDeleteHandler readingBlockDeleteHandler,
            IReadingBlockGetSingleHandler readingBlockGetSingleHandler,
            IReadingBlockGetAllHandler readingBlockGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBlockCreateHandler = readingBlockCreateHandler;
            _readingBlockCreateHandler.NotNull(nameof(readingBlockCreateHandler));

            _readingBlockUpdateHandler = readingBlockUpdateHandler;
            _readingBlockUpdateHandler.NotNull(nameof(readingBlockUpdateHandler));

            _readingBlockDeleteHandler = readingBlockDeleteHandler;
            _readingBlockDeleteHandler.NotNull(nameof(readingBlockDeleteHandler));

            _readingBlockGetSingleHandler = readingBlockGetSingleHandler;
            _readingBlockGetSingleHandler.NotNull(nameof(readingBlockGetSingleHandler));

            _readingBlockGetAllHandler = readingBlockGetAllHandler;
            _readingBlockGetAllHandler.NotNull(nameof(readingBlockGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingBlockUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingBlockUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBlockDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingBlockDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingBlockDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBlockGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingBlockGetDto readingBlock = await _readingBlockGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingBlock);
        }

        [HttpGet, HttpPost]
        [Route("all/{readingBoundId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBlockGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int readingBoundId,CancellationToken cancellationToken)
        {
            ICollection<ReadingBlockGetDto> readingBlocks = await _readingBlockGetAllHandler.Handle(readingBoundId,cancellationToken);
            return Ok(readingBlocks);
        }
    }
}
