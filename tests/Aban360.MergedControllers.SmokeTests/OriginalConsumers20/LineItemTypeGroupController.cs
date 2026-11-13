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
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeGroupCreateHandler _lineItemTypeGroupCreateHandler;
        private readonly ILineItemTypeGroupUpdateHandler _lineItemTypeGroupUpdateHandler;
        private readonly ILineItemTypeGroupDeleteHandler _lineItemTypeGroupDeleteHandler;
        private readonly ILineItemTypeGroupGetSingleHandler _lineItemTypeGroupGetSingleHandler;
        private readonly ILineItemTypeGroupGetAllHandler _lineItemTypeGroupGetAllHandler;

        public LineItemTypeGroupController(
            IUnitOfWork uow,
            ILineItemTypeGroupCreateHandler lineItemTypeGroupCreateHandler,
            ILineItemTypeGroupUpdateHandler lineItemTypeGroupUpdateHandler,
            ILineItemTypeGroupDeleteHandler lineItemTypeGroupDeleteHandler,
            ILineItemTypeGroupGetSingleHandler lineItemTypeGroupGetSingleHandler,
            ILineItemTypeGroupGetAllHandler LineItemTypeGroupGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeGroupCreateHandler = lineItemTypeGroupCreateHandler;
            _lineItemTypeGroupCreateHandler.NotNull(nameof(lineItemTypeGroupCreateHandler));

            _lineItemTypeGroupUpdateHandler = lineItemTypeGroupUpdateHandler;
            _lineItemTypeGroupUpdateHandler.NotNull(nameof(lineItemTypeGroupUpdateHandler));

            _lineItemTypeGroupDeleteHandler = lineItemTypeGroupDeleteHandler;
            _lineItemTypeGroupDeleteHandler.NotNull(nameof(lineItemTypeGroupDeleteHandler));

            _lineItemTypeGroupGetSingleHandler = lineItemTypeGroupGetSingleHandler;
            _lineItemTypeGroupGetSingleHandler.NotNull(nameof(lineItemTypeGroupGetSingleHandler));

            _lineItemTypeGroupGetAllHandler = LineItemTypeGroupGetAllHandler;
            _lineItemTypeGroupGetAllHandler.NotNull(nameof(LineItemTypeGroupGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] LineItemTypeGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeGroupCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] LineItemTypeGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeGroupUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] LineItemTypeGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _lineItemTypeGroupDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LineItemTypeGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            LineItemTypeGroupGetDto lineItemTypeGroups = await _lineItemTypeGroupGetSingleHandler.Handle(id, cancellationToken);
            return Ok(lineItemTypeGroups);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<LineItemTypeGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<LineItemTypeGroupGetDto> lineItemTypeGroups = await _lineItemTypeGroupGetAllHandler.Handle(cancellationToken);
            return Ok(lineItemTypeGroups);
        }
    }
}
