using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Commands
{
    [Route("v1/water-return")]
    public class ReturnBillDeleteController : BaseController
    {
        private readonly IReturnBillUncofirmedDeleteHandler _returnDeleteHandler;
        public ReturnBillDeleteController(IReturnBillUncofirmedDeleteHandler returnDeleteHandler)
        {
            _returnDeleteHandler = returnDeleteHandler;
            _returnDeleteHandler.NotNull(nameof(returnDeleteHandler));
        }

        [HttpPost, HttpGet]
        [Route("delete/{confirmedNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnconfirmedDelete(int confirmedNumber, CancellationToken cancellationToken)
        {
            await _returnDeleteHandler.Handle(confirmedNumber, CurrentUser, cancellationToken); 
            return Ok(confirmedNumber);
        }
    }
}
