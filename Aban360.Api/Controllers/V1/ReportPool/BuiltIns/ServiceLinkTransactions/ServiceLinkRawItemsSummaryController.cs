using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link-items-summary")]
    public class ServiceLinkRawItemsSummaryController : BaseController
    {
        private readonly IServiceLinkRawItemsSummaryHandler _serviceLinkRawItemsSummaryHandler;
        public ServiceLinkRawItemsSummaryController(IServiceLinkRawItemsSummaryHandler serviceLinkRawItemsSummaryHandler)
        {
            _serviceLinkRawItemsSummaryHandler = serviceLinkRawItemsSummaryHandler;
            _serviceLinkRawItemsSummaryHandler.NotNull(nameof(serviceLinkRawItemsSummaryHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkRawItemsInputDto input, CancellationToken cancellationToken)
        {
            var result = await _serviceLinkRawItemsSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
