using Aban360.MeterPool.Application.Features.Management.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;




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
        private readonly IReadingConfigDefaultService _service;

        public ReadingConfigDefaultController(IUnitOfWork uow, IReadingConfigDefaultService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReadingConfigDefaultCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ReadingConfigDefaultUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReadingConfigDefaultGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ReadingConfigDefaultGetDto readingConfigDefaults = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(readingConfigDefaults);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingConfigDefaultGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingConfigDefaultGetDto> readingConfigDefaults = await _service.GetAllAsync(cancellationToken);
            return Ok(readingConfigDefaults);
        }
    }
}
