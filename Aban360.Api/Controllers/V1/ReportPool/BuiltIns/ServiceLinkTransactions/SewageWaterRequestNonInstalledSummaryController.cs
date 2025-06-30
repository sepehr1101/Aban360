using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-request-noninstalled-Summary")]
    public class SewageWaterRequestNonInstalledSummaryController : BaseController
    {
        private readonly ISewageWaterRequestNonInstalledSummaryHandler _sewageWaterRequestNonInstalledSummaryHandler;
        public SewageWaterRequestNonInstalledSummaryController(ISewageWaterRequestNonInstalledSummaryHandler sewageWaterRequestNonInstalledSummaryHandler)
        {
            _sewageWaterRequestNonInstalledSummaryHandler = sewageWaterRequestNonInstalledSummaryHandler;
            _sewageWaterRequestNonInstalledSummaryHandler.NotNull(nameof(sewageWaterRequestNonInstalledSummaryHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken)
        {
            var result = await _sewageWaterRequestNonInstalledSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
