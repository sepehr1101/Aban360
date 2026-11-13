using Aban360.CalculationPool.Application.Features.Bill.Services;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeGroupService _service;

        public LineItemTypeGroupController(IUnitOfWork uow, ILineItemTypeGroupService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] LineItemTypeGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] LineItemTypeGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            LineItemTypeGroupGetDto lineItemTypeGroups = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(lineItemTypeGroups);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<LineItemTypeGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<LineItemTypeGroupGetDto> lineItemTypeGroups = await _service.GetAllAsync(cancellationToken);
            return Ok(lineItemTypeGroups);
        }
    }
}
