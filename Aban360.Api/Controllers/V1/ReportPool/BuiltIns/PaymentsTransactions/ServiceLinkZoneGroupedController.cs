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
    [Route("v1/service-link-zone-grouped")]
    public class ServiceLinkZoneGroupedController : BaseController
    {
        private readonly IServiceLinkZoneGroupedHandler _serviceLinkZoneGrouped;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkZoneGroupedController(
            IServiceLinkZoneGroupedHandler serviceLinkZoneGrouped,
            IReportGenerator reportGenerator)
        {
            _serviceLinkZoneGrouped = serviceLinkZoneGrouped;
            _serviceLinkZoneGrouped.NotNull(nameof(_serviceLinkZoneGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto> ServiceLinkZoneGrouped = await _serviceLinkZoneGrouped.Handle(inputDto, cancellationToken);
            return Ok(ServiceLinkZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _serviceLinkZoneGrouped.Handle, CurrentUser, ReportLiterals.ServiceLinkZoneGrouped, connectionId);
            return Ok(inputDto);
        }
    }
}
