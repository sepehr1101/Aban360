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
    [Route("v1/water-usage-grouped")]
    public class WaterUsageGroupedController : BaseController
    {
        private readonly IWaterUsageGroupedHandler _waterUsageGrouped;
        private readonly IReportGenerator _reportGenerator;
        public WaterUsageGroupedController(
            IWaterUsageGroupedHandler waterUsageGrouped,
            IReportGenerator reportGenerator)
        {
            _waterUsageGrouped = waterUsageGrouped;
            _waterUsageGrouped.NotNull(nameof(_waterUsageGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto> waterUsageGrouped = await _waterUsageGrouped.Handle(inputDto, cancellationToken);
            return Ok(waterUsageGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _waterUsageGrouped.Handle, CurrentUser, ReportLiterals.WaterUsageGrouped, connectionId);
            return Ok(inputDto);
        }
    }
}
