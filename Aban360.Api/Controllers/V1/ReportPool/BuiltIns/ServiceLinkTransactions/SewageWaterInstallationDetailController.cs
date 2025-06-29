using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-installation-detail")]
    public class SewageWaterInstallationDetailController : BaseController
    {
        private readonly ISewageWaterInstallationDetailHandler _sewageWaterInstallationDetailHandler;
        public SewageWaterInstallationDetailController(ISewageWaterInstallationDetailHandler sewageWaterInstallationDetailHandler)
        {
            _sewageWaterInstallationDetailHandler = sewageWaterInstallationDetailHandler;
            _sewageWaterInstallationDetailHandler.NotNull(nameof(sewageWaterInstallationDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationInputDto input,CancellationToken cancellationToken)
        {
            var result=await _sewageWaterInstallationDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
