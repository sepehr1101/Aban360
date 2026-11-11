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

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/reading-bound")]
    public class ReadingBoundeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBoundCreateHandler _readingBoundCreateHandler;
        private readonly IReadingBoundUpdateHandler _readingBoundUpdateHandler;
        private readonly IReadingBoundIsValidateUpdateHandler _readingBoundIsValidateUpdateHandler;
        private readonly IReadingBoundDeleteHandler _readingBoundDeleteHandler;
        private readonly IReadingBoundGetSingleHandler _readingBoundGetSingleHandler;
        private readonly IReadingBoundGetAllHandler _readingBoundGetAllHandler;
        private readonly IReadingBoundGetByZoneIdHandler _readingBoundGetByZoneIdHandler;

        public ReadingBoundeController(
            IUnitOfWork uow,
            IReadingBoundCreateHandler readingBoundCreateHandler,
            IReadingBoundUpdateHandler readingBoundUpdateHandler,
            IReadingBoundIsValidateUpdateHandler readingBoundIsValidateUpdateHandler,
            IReadingBoundDeleteHandler readingBoundDeleteHandler,
            IReadingBoundGetSingleHandler readingBoundGetSingleHandler,
            IReadingBoundGetAllHandler readingBoundGetAllHandler,
            IReadingBoundGetByZoneIdHandler readingBoundGetByZoneIdHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBoundCreateHandler = readingBoundCreateHandler;
            _readingBoundCreateHandler.NotNull(nameof(readingBoundCreateHandler));

            _readingBoundUpdateHandler = readingBoundUpdateHandler;
            _readingBoundUpdateHandler.NotNull(nameof(readingBoundUpdateHandler));

            _readingBoundIsValidateUpdateHandler = readingBoundIsValidateUpdateHandler;
            _readingBoundIsValidateUpdateHandler.NotNull(nameof(readingBoundIsValidateUpdateHandler));

            _readingBoundDeleteHandler = readingBoundDeleteHandler;
            _readingBoundDeleteHandler.NotNull(nameof(readingBoundDeleteHandler));

            _readingBoundGetSingleHandler = readingBoundGetSingleHandler;
            _readingBoundGetSingleHandler.NotNull(nameof(readingBoundGetSingleHandler));

            _readingBoundGetAllHandler = readingBoundGetAllHandler;
            _readingBoundGetAllHandler.NotNull(nameof(readingBoundGetAllHandler));

            _readingBoundGetByZoneIdHandler = readingBoundGetByZoneIdHandler;
            _readingBoundGetByZoneIdHandler.NotNull(nameof(readingBoundGetByZoneIdHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBoundCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingBoundCreateDto createDto, CancellationToken cancellationToken)
        {
            await _readingBoundCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBoundUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingBoundUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _readingBoundUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }


        [HttpPost, HttpPatch]
        [Route("update-isvalid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBoundIsValidUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsValidUpdate([FromBody] ReadingBoundIsValidUpdateDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                await _readingBoundIsValidateUpdateHandler.Handle(updateDto, cancellationToken);

            }
            catch
            {
                await _uow.SaveChangesAsync(cancellationToken);
                throw;
            }
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBoundDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingBoundDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _readingBoundDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingBoundGetDto readingBound = await _readingBoundGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingBound);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingBoundGetDto> readingBound = await _readingBoundGetAllHandler.Handle(cancellationToken);
            return Ok(readingBound);
        }

        [HttpGet, HttpPost]
        [Route("get-by-zone/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByZoneId(int zoneId, CancellationToken cancellationToken)
        {
            ICollection<ReadingBoundGetDto> readingBound = await _readingBoundGetByZoneIdHandler.Handle(zoneId, cancellationToken);
            return Ok(readingBound);
        }
    }
}
