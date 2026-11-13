using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.BrdigeApi.Controllers.V1.CalculationPool;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    public class BillReturnController : BaseController
    {
        private readonly IReturnBillSetConfirmHandler _c0_returnConfirmHandler;
        private readonly IReturnBillPartialHandler _c1_billToReturnedHandler;
        private readonly IReturnBillFullHandler _c1_billFullHandler;
        public BillReturnController(IReturnBillSetConfirmHandler c0_returnConfirmHandler, IReturnBillPartialHandler c1_billToReturnedHandler, IReturnBillFullHandler c1_billFullHandler)
        {
            _c0_returnConfirmHandler = c0_returnConfirmHandler;
            _c0_returnConfirmHandler.NotNull(nameof(c0_returnConfirmHandler));
            _c1_billToReturnedHandler = c1_billToReturnedHandler;
            _c1_billToReturnedHandler.NotNull(nameof(c1_billToReturnedHandler));
            _c1_billFullHandler = c1_billFullHandler;
            _c1_billFullHandler.NotNull(nameof(c1_billFullHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bill-return/confirm")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm([FromBody] ReturnBillSetConfirmInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _c0_returnConfirmHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bill-return/partial")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PartialReturn([FromBody] ReturnBillPartialInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _c1_billToReturnedHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/bill-return/full")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FullReturn([FromBody] ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            FlatReportOutput<ReturnBillHeaderOutputDto, ReturnBillOutputDto> result = await _c1_billFullHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
