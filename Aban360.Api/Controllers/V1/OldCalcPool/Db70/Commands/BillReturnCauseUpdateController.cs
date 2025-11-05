using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Commands
{
    [Route("v1/bill-return-cause")]
    public class BillReturnCauseUpdateController : BaseController
    {
        private readonly IBillReturnCauseUpdateHandler _billReturnCauseHandler;
        public BillReturnCauseUpdateController(IBillReturnCauseUpdateHandler billReturnCauseHandler)
        {
            _billReturnCauseHandler = billReturnCauseHandler;
            _billReturnCauseHandler.NotNull(nameof(billReturnCauseHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(BillReturnCauseUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseHandler.Handle(updateDto, cancellationToken);
            return Ok(updateDto);
        }
    }
}
