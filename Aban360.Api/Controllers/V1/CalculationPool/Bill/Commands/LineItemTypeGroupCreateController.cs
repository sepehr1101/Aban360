using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/line-item-type-group")]
    public class LineItemTypeGroupCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeGroupCreateHandler _lineItemTypeGroupCreateHandler;
        public LineItemTypeGroupCreateController(
            IUnitOfWork uow,
            ILineItemTypeGroupCreateHandler lineItemTypeGroupCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeGroupCreateHandler = lineItemTypeGroupCreateHandler;
            _lineItemTypeGroupCreateHandler.NotNull(nameof(lineItemTypeGroupCreateHandler));
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
    }
}
