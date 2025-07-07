using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link-net-items-detail")]
    public class ServiceLinkNetItemsDetailController : BaseController
    {
        private readonly IServiceLinkNetItemsDetailHandler _serviceLinkNetItemsDetailHandler;
        public ServiceLinkNetItemsDetailController(IServiceLinkNetItemsDetailHandler serviceLinkNetItemsDetailHandler)
        {
            _serviceLinkNetItemsDetailHandler = serviceLinkNetItemsDetailHandler;
            _serviceLinkNetItemsDetailHandler.NotNull(nameof(serviceLinkNetItemsDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("Net")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNet(ServiceLinkNetItemsInputDto input, CancellationToken cancellationToken)
        {
            var result = await _serviceLinkNetItemsDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
