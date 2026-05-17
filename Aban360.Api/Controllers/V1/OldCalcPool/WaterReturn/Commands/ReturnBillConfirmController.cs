using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Commands
{
    [Route("v1/water-return")]
    public class ReturnBillConfirmController : BaseController
    {
        private readonly IReturnBillConfirmeHandler _returnConfirmHandler;
        private readonly IReturnBillSetConfirmHandler _returnSetConfirmHandler;
        public ReturnBillConfirmController(
            IReturnBillConfirmeHandler returnConfirmHandler,
            IReturnBillSetConfirmHandler returnSetConfirmHandler)
        {
            _returnConfirmHandler = returnConfirmHandler;
            _returnConfirmHandler.NotNull(nameof(returnConfirmHandler));

            _returnSetConfirmHandler = returnSetConfirmHandler;
            _returnSetConfirmHandler.NotNull(nameof(returnSetConfirmHandler));
        }

        //[HttpPost, HttpGet]
        //[Route("confirm")]
        //[ProducesResponseType(typeof(ApiResponseEnvelope<ReturnBillDataOutputDto>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> Confirm([FromBody] ReturnBillConfirmeByBillIdInputDto input, CancellationToken cancellationToken)
        //{
        //    ReturnBillDataOutputDto result = await _returnConfirmHandler.Handle(input, cancellationToken);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm([FromBody] ReturnBillSetConfirmInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _returnSetConfirmHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}