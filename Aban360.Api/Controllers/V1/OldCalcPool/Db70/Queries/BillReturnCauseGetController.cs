using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70.Queries
{
    [Route("v1/bill-return-cause")]
    public class BillReturnCauseGetController : BaseController
    {
        private readonly IBillReturnCauseGetHandler _billReturnCauseHandler;
        public BillReturnCauseGetController(IBillReturnCauseGetHandler billReturnCauseHandler)
        {
            _billReturnCauseHandler = billReturnCauseHandler;
            _billReturnCauseHandler.NotNull(nameof(billReturnCauseHandler));
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(SearchShortInputDto inputDto, CancellationToken cancellationToken)
        {
            BillReturnCauseGetDto result = await _billReturnCauseHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
