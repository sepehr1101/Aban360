using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link-net-items-summary")]
    public class ServiceLinkNetItemsSummaryController : BaseController
    {
        private readonly IServiceLinkNetItemsSummaryHandler _serviceLinkNetItemsSummaryHandler;
        public ServiceLinkNetItemsSummaryController(IServiceLinkNetItemsSummaryHandler serviceLinkNetItemsSummaryHandler)
        {
            _serviceLinkNetItemsSummaryHandler = serviceLinkNetItemsSummaryHandler;
            _serviceLinkNetItemsSummaryHandler.NotNull(nameof(serviceLinkNetItemsSummaryHandler));
        }

        [HttpPost, HttpGet]
        [Route("Net")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNet(ServiceLinkNetItemsInputDto input, CancellationToken cancellationToken)
        {
            var result = await _serviceLinkNetItemsSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
