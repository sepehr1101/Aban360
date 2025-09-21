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
    [Route("v1/sewage-zone-grouped")]
    public class SewageZoneGroupedController : BaseController
    {
        private readonly ISewageZoneGroupedHandler _sewageZoneGrouped;
        private readonly IReportGenerator _reportGenerator;
        public SewageZoneGroupedController(
            ISewageZoneGroupedHandler sewageZoneGrouped,
            IReportGenerator reportGenerator)
        {
            _sewageZoneGrouped = sewageZoneGrouped;
            _sewageZoneGrouped.NotNull(nameof(_sewageZoneGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterItemGroupedHeaderOutputDto, SewageWaterItemGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterItemGroupedHeaderOutputDto, SewageWaterItemGroupedDataOutputDto> SewageZoneGrouped = await _sewageZoneGrouped.Handle(inputDto, cancellationToken);
            return Ok(SewageZoneGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterItemGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageZoneGrouped.Handle, CurrentUser, ReportLiterals.SewageZoneGrouped, connectionId);
            return Ok(inputDto);
        }
    }
}
