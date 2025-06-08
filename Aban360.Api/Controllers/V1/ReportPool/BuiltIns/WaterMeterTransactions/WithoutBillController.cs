using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/without-bill")]
    public class WithoutBillController : BaseController
    {
        private readonly IWithoutBillHandler _withoutBillHandler;
        public WithoutBillController(IWithoutBillHandler withoutBillHandler)
        {
            _withoutBillHandler = withoutBillHandler;
            _withoutBillHandler.NotNull(nameof(withoutBillHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutBillInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WithoutBillHeaderOutputDto, WithoutBillDataOutputDto> withoutBill = await _withoutBillHandler.Handle(input, cancellationToken);
            return Ok(withoutBill);
        }
    }
}
