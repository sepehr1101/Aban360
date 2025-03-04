using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/line-item-type")]
    public class LineItemTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILineItemTypeCreateHandler _lineItemTypeCreateHandler;
        public LineItemTypeCreateController(
            IUnitOfWork uow,
            ILineItemTypeCreateHandler lineItemTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _lineItemTypeCreateHandler = lineItemTypeCreateHandler;
            _lineItemTypeCreateHandler.NotNull(nameof(lineItemTypeCreateHandler));
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
    }
}
