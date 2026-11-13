using Aban360.ClaimPool.Application.Features.People.Services;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;

using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-type")]
    public class IndividualTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTypeService _service;

        public IndividualTypeController(IUnitOfWork uow, IIndividualTypeService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            IndividualTypeGetDto IndividualType = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(IndividualType);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualTypeGetDto> IndividualType = await _service.GetAllAsync(cancellationToken);
            return Ok(IndividualType);
        }
    }
}
