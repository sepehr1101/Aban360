using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link-items-detail")]
    public class ServiceLinkRawItemsDetailController : BaseController
    {
        private readonly IServiceLinkRawItemsDetailHandler _serviceLinkRawItemsDetailHandler;
        public ServiceLinkRawItemsDetailController(IServiceLinkRawItemsDetailHandler serviceLinkRawItemsDetailHandler)
        {
            _serviceLinkRawItemsDetailHandler = serviceLinkRawItemsDetailHandler;
            _serviceLinkRawItemsDetailHandler.NotNull(nameof(serviceLinkRawItemsDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkRawItemsInputDto input, CancellationToken cancellationToken)
        {
            var result = await _serviceLinkRawItemsDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
