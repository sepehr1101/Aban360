using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
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
        private readonly IReadingBoundService _service;
        public ReadingBoundeController(IUnitOfWork uow, IReadingBoundService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBoundCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingBoundCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingBoundUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingBoundUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
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
                await _service.ValidateAsync(updateDto, cancellationToken);

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
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingBoundGetDto readingBound = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(readingBound);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingBoundGetDto> readingBound = await _service.GetAllAsync(cancellationToken);
            return Ok(readingBound);
        }

        [HttpGet, HttpPost]
        [Route("get-by-zone/{zoneId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBoundGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByZoneId(int zoneId, CancellationToken cancellationToken)
        {
            ICollection<ReadingBoundGetDto> readingBound = await _service.GetByZoneIdAsync(zoneId, cancellationToken);
            return Ok(readingBound);
        }
    }
}
