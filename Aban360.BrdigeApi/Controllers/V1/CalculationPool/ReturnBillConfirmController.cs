using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill-return")]
    public class ReturnBillConfirmController : BaseController
    {
        private readonly IReturnBillSetConfirmHandler _returnConfirmHandler;
        public ReturnBillConfirmController(IReturnBillSetConfirmHandler returnConfirmHandler)
        {
            _returnConfirmHandler = returnConfirmHandler;
            _returnConfirmHandler.NotNull(nameof(returnConfirmHandler));
        }

        [HttpPost, HttpGet]
        [Route("confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm([FromBody] ReturnBillSetConfirmInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _returnConfirmHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
