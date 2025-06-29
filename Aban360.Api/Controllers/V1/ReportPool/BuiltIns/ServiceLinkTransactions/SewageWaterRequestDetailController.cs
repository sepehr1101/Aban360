using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-request-detail")]
    public class SewageWaterRequestDetailController : BaseController
    {
        private readonly ISewageWaterRequestDetailHandler _sewageWaterRequestDetailHandler;
        public SewageWaterRequestDetailController(ISewageWaterRequestDetailHandler sewageWaterRequestDetailHandler)
        {
            _sewageWaterRequestDetailHandler = sewageWaterRequestDetailHandler;
            _sewageWaterRequestDetailHandler.NotNull(nameof(sewageWaterRequestDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterRequestInputDto input,CancellationToken cancellationToken)
        {
            var result=await _sewageWaterRequestDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
