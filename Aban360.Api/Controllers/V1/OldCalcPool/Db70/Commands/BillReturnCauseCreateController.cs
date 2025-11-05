using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Commands
{
    [Route("v1/bill-return-cause")]
    public class BillReturnCauseCreateController : BaseController
    {
        private readonly IBillReturnCauseCreateHandler _billReturnCauseHandler;
        public BillReturnCauseCreateController(IBillReturnCauseCreateHandler billReturnCauseHandler)
        {
            _billReturnCauseHandler = billReturnCauseHandler;
            _billReturnCauseHandler.NotNull(nameof(billReturnCauseHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(BillReturnCauseCreateDto createDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseHandler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }
    }
}
