using Aban360.Api.Cronjobs;
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
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkNetItemsDetailController(
            IServiceLinkNetItemsDetailHandler serviceLinkNetItemsDetailHandler,
            IReportGenerator reportGenerator)
        {
            _serviceLinkNetItemsDetailHandler = serviceLinkNetItemsDetailHandler;
            _serviceLinkNetItemsDetailHandler.NotNull(nameof(serviceLinkNetItemsDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("Net")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNet(ServiceLinkNetItemsInputDto input, CancellationToken cancellationToken)
        {
            var result = await _serviceLinkNetItemsDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkNetItemsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkNetItemsDetailHandler.Handle, CurrentUser, ReportLiterals.ServiceLinkNetItemsDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
