using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
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
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkNetItemsSummaryController(
            IServiceLinkNetItemsSummaryHandler serviceLinkNetItemsSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _serviceLinkNetItemsSummaryHandler = serviceLinkNetItemsSummaryHandler;
            _serviceLinkNetItemsSummaryHandler.NotNull(nameof(serviceLinkNetItemsSummaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("Net")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNet(ServiceLinkNetItemsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto> result = await _serviceLinkNetItemsSummaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkNetItemsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkNetItemsSummaryHandler.Handle, CurrentUser, ReportLiterals.ServiceLinkNetItemsSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ServiceLinkNetItemsInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 618;
            ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto> result = await _serviceLinkNetItemsSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
