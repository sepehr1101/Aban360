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
    [Route("v1/sewage-usage-grouped")]
    public class SewageUsageGroupedController : BaseController
    {
        private readonly ISewageUsageGroupedHandler _sewageUsageGrouped;
        private readonly IReportGenerator _reportGenerator;
        public SewageUsageGroupedController(
            ISewageUsageGroupedHandler sewageUsageGrouped,
            IReportGenerator reportGenerator)
        {
            _sewageUsageGrouped = sewageUsageGrouped;
            _sewageUsageGrouped.NotNull(nameof(_sewageUsageGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterItemGroupedHeaderOutputDto, SewageWaterItemGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterItemGroupedHeaderOutputDto, SewageWaterItemGroupedDataOutputDto> SewageUsageGrouped = await _sewageUsageGrouped.Handle(inputDto, cancellationToken);
            return Ok(SewageUsageGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageUsageGrouped.Handle, CurrentUser, ReportLiterals.SewageUsageGrouped, connectionId);
            return Ok(inputDto);
        }
    }
}
