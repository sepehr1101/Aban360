using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-modified-bills-summary")]
    public class WaterModifiedBillsSummaryController : BaseController
    {
        private readonly IWaterModifiedBillsSummaryHandler _modifiedBillsHandler;
        public WaterModifiedBillsSummaryController(IWaterModifiedBillsSummaryHandler modifiedBillsHandler)
        {
            _modifiedBillsHandler = modifiedBillsHandler;
            _modifiedBillsHandler.NotNull(nameof(modifiedBillsHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterModifiedBillsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsSummaryDataOutputDto> modifiedBills = await _modifiedBillsHandler.Handle(input, cancellationToken);
            return Ok(modifiedBills);
        }
    }
}
