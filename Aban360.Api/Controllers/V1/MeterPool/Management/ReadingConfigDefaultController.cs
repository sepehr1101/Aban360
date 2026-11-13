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
    [Route("v1/reading-config-default")]
    public class ReadingConfigDefaultController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingConfigDefaultCreateHandler _readingConfigDefaultCreateHandler;
        private readonly IReadingConfigDefaultUpdateHandler _readingConfigDefaultUpdateHandler;
        private readonly IReadingConfigDefaultDeleteHandler _readingConfigDefaultDeleteHandler;
        private readonly IReadingConfigDefaultGetSingleHandler _readingConfigDefaultGetSingleHandler;
        private readonly IReadingConfigDefaultGetAllHandler _readingConfigDefaultGetAllHandler;

        public ReadingConfigDefaultController(
            IUnitOfWork uow,
            IReadingConfigDefaultCreateHandler readingConfigDefaultCreateHandler,
            IReadingConfigDefaultUpdateHandler readingConfigDefaultUpdateHandler,
            IReadingConfigDefaultDeleteHandler readingConfigDefaultDeleteHandler,
            IReadingConfigDefaultGetSingleHandler readingConfigDefaultGetSingleHandler,
            IReadingConfigDefaultGetAllHandler readingConfigDefaultGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingConfigDefaultCreateHandler = readingConfigDefaultCreateHandler;
            _readingConfigDefaultCreateHandler.NotNull(nameof(readingConfigDefaultCreateHandler));

            _readingConfigDefaultUpdateHandler = readingConfigDefaultUpdateHandler;
            _readingConfigDefaultUpdateHandler.NotNull(nameof(readingConfigDefaultUpdateHandler));

            _readingConfigDefaultDeleteHandler = readingConfigDefaultDeleteHandler;
            _readingConfigDefaultDeleteHandler.NotNull(nameof(readingConfigDefaultDeleteHandler));

            _readingConfigDefaultGetSingleHandler = readingConfigDefaultGetSingleHandler;
            _readingConfigDefaultGetSingleHandler.NotNull(nameof(readingConfigDefaultGetSingleHandler));

            _readingConfigDefaultGetAllHandler = readingConfigDefaultGetAllHandler;
            _readingConfigDefaultGetAllHandler.NotNull(nameof(readingConfigDefaultGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingConfigDefaultUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingConfigDefaultUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingConfigDefaultDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingConfigDefaultGetDto readingConfigDefaults = await _readingConfigDefaultGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingConfigDefaults);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingConfigDefaultGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingConfigDefaultGetDto> readingConfigDefaults = await _readingConfigDefaultGetAllHandler.Handle(cancellationToken);
            return Ok(readingConfigDefaults);
        }
    }
}
