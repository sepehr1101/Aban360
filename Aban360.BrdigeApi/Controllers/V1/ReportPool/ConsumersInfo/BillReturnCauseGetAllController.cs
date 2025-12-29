using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillReturnCauseGetAllController : BaseController
    {
        private readonly IBillReturnCauseGetAllHandler _billReturnCauseHandler;
        public BillReturnCauseGetAllController(IBillReturnCauseGetAllHandler billReturnCauseHandler)
        {
            _billReturnCauseHandler = billReturnCauseHandler;
            _billReturnCauseHandler.NotNull(nameof(billReturnCauseHandler));
        }

        [HttpGet]
        [Route("return-cause")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<BillReturnCauseGetDto> result = await _billReturnCauseHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
