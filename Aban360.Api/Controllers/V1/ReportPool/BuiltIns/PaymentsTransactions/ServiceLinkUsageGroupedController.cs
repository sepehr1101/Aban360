using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/service-link-usage-grouped")]
    public class ServiceLinkUsageGroupedController : BaseController
    {
        private readonly IServiceLinkUsageGroupedHandler _serviceLinkUsageGrouped;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkUsageGroupedController(
            IServiceLinkUsageGroupedHandler serviceLinkUsageGrouped,
            IReportGenerator reportGenerator)
        {
            _serviceLinkUsageGrouped = serviceLinkUsageGrouped;
            _serviceLinkUsageGrouped.NotNull(nameof(_serviceLinkUsageGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto> ServiceLinkUsageGrouped = await _serviceLinkUsageGrouped.Handle(inputDto, cancellationToken);
            return Ok(ServiceLinkUsageGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkUsageGrouped.Handle, CurrentUser, ReportLiterals.ServiceLinkUsageGrouped, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 481;
            ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto> result = await _serviceLinkUsageGrouped.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
