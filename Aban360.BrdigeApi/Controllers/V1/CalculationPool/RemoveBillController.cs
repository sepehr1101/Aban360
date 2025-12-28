using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill")]
    public class RemoveBillController : BaseController
    {
        private readonly IRemoveBillHandler _removedBillHandler;
        public RemoveBillController(IRemoveBillHandler removedBillHandler)
        {
            _removedBillHandler = removedBillHandler;
            _removedBillHandler.NotNull(nameof(removedBillHandler));
        }

        [HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] IdWrapper idWrapper , CancellationToken cancellationToken)
        {
            await _removedBillHandler.Handle(idWrapper.Id, cancellationToken);
            return Ok(idWrapper);
        }
    }
}
