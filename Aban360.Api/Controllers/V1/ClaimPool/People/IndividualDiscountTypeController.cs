using Aban360.ClaimPool.Application.Features.People.Services;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/individual-discount-type")]
    public class IndividualDiscountTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualDiscountTypeService _service;

        public IndividualDiscountTypeController(IUnitOfWork uow, IIndividualDiscountTypeService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var individualDiscountTypes = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(individualDiscountTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualDiscountTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var individualDiscountTypes = await _service.GetAllAsync(cancellationToken);
            return Ok(individualDiscountTypes);
        }
    }
}
