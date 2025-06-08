using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/modified-bills")]
    public class ModifiedBillsController : BaseController
    {
        private readonly IModifiedBillsHandler _modifiedBillsHandler;
        public ModifiedBillsController(IModifiedBillsHandler modifiedBillsHandler)
        {
            _modifiedBillsHandler = modifiedBillsHandler;
            _modifiedBillsHandler.NotNull(nameof(modifiedBillsHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ModifiedBillsHeaderOutputDto, ModifiedBillsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ModifiedBillsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ModifiedBillsHeaderOutputDto, ModifiedBillsDataOutputDto> modifiedBills = await _modifiedBillsHandler.Handle(input, cancellationToken);
            return Ok(modifiedBills);
        }
    }
}
