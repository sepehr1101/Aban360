using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Commands
{
    [Route("v1/bill-return-cause")]
    public class BillReturnCauseDeleteController : BaseController
    {
        private readonly IBillReturnCauseDeleteHandler _billReturnCauseHandler;
        public BillReturnCauseDeleteController(IBillReturnCauseDeleteHandler billReturnCauseHandler)
        {
            _billReturnCauseHandler = billReturnCauseHandler;
            _billReturnCauseHandler.NotNull(nameof(billReturnCauseHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(BillReturnCauseDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseHandler.Handle(deleteDto, CurrentUser, cancellationToken);
            return Ok(deleteDto);
        }
    }
}
