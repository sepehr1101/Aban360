// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.CalculationPool.Bill;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/line-item-type")]
    public class LineItemTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeCreateHandler _lineItemTypeCreateHandler;
        private readonly ILineItemTypeUpdateHandler _lineItemTypeUpdateHandler;
        private readonly ILineItemTypeDeleteHandler _lineItemTypeDeleteHandler;
        private readonly ILineItemTypeGetSingleHandler _lineItemTypeGetSingleHandler;
        private readonly ILineItemTypeGetAllHandler _lineItemTypeGetAllHandler;

        public LineItemTypeController(
            IUnitOfWork uow,
            ILineItemTypeCreateHandler lineItemTypeCreateHandler,
            ILineItemTypeUpdateHandler lineItemTypeUpdateHandler,
            ILineItemTypeDeleteHandler lineItemTypeDeleteHandler,
            ILineItemTypeGetSingleHandler lineItemTypeGetSingleHandler,
            ILineItemTypeGetAllHandler LineItemTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeCreateHandler = lineItemTypeCreateHandler;
            _lineItemTypeCreateHandler.NotNull(nameof(lineItemTypeCreateHandler));

            _lineItemTypeUpdateHandler = lineItemTypeUpdateHandler;
            _lineItemTypeUpdateHandler.NotNull(nameof(lineItemTypeUpdateHandler));

            _lineItemTypeDeleteHandler = lineItemTypeDeleteHandler;
            _lineItemTypeDeleteHandler.NotNull(nameof(lineItemTypeDeleteHandler));

            _lineItemTypeGetSingleHandler = lineItemTypeGetSingleHandler;
            _lineItemTypeGetSingleHandler.NotNull(nameof(lineItemTypeGetSingleHandler));

            _lineItemTypeGetAllHandler = LineItemTypeGetAllHandler;
            _lineItemTypeGetAllHandler.NotNull(nameof(LineItemTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] LineItemTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] LineItemTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] LineItemTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            LineItemTypeGetDto lineItemTypes = await _lineItemTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(lineItemTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<LineItemTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<LineItemTypeGetDto> lineItemTypes = await _lineItemTypeGetAllHandler.Handle(cancellationToken);
            return Ok(lineItemTypes);
        }
    }
}
