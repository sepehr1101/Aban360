using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
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
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
        {
            await _removedBillHandler.Handle(id, cancellationToken);
            return Ok(id);
        }
    }
}
