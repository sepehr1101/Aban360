using Aban360.ClaimPool.Application.Features.Land.Services;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/falt")]
    public class FlatController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatService _service;
        public FlatController(IUnitOfWork uow, IFlatService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FlatCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] FlatUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] FlatDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            FlatGetDto flat = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(flat);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<FlatGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<FlatGetDto> flat = await _service.GetAllAsync(cancellationToken);
            return Ok(flat);
        }
    }
}
