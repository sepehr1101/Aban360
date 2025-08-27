using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link-raw-items-detail")]
    public class ServiceLinkRawItemsDetailController : BaseController
    {
        private readonly IServiceLinkRawItemsDetailHandler _serviceLinkRawItemsDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkRawItemsDetailController(
            IServiceLinkRawItemsDetailHandler serviceLinkRawItemsDetailHandler,
            IReportGenerator reportGenerator)
        {
            _serviceLinkRawItemsDetailHandler = serviceLinkRawItemsDetailHandler;
            _serviceLinkRawItemsDetailHandler.NotNull(nameof(serviceLinkRawItemsDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkRawItemsInputDto input, CancellationToken cancellationToken)
        {
            var result = await _serviceLinkRawItemsDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkRawItemsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkRawItemsDetailHandler.Handle, CurrentUser, ReportLiterals.ServiceLinkRawItemsDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
