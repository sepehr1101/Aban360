using Aban360.CalculationPool.Application.Features.Bill.Services;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;

using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/offering-group")]
    public class OfferingGroupController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupService _service;

        public OfferingGroupController(IUnitOfWork uow, IOfferingGroupService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfferingGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            OfferingGroupGetDto offeringGroups = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(offeringGroups);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<OfferingGroupGetDto> offeringGroups = await _service.GetAllAsync(cancellationToken);
            return Ok(offeringGroups);
        }
    }
}
