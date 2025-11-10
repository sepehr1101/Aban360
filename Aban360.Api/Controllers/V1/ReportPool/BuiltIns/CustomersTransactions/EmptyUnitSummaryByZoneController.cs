using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/empty-unit-summary-by-zone")]
    public class EmptyUnitSummaryByZoneController : BaseController
    {
        private readonly IEmptyUnitSummaryByZoneHandler _emptyUnit;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitSummaryByZoneController(
            IEmptyUnitSummaryByZoneHandler emptyUnit,
            IReportGenerator reportGenerator)
        {
            _emptyUnit = emptyUnit;
            _emptyUnit.NotNull(nameof(_emptyUnit));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitSummaryHeaderOutputDto, EmptyUnitSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitSummaryHeaderOutputDto, EmptyUnitSummaryDataOutputDto> emptyUnit = await _emptyUnit.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnit.Handle, CurrentUser, ReportLiterals.EmptyUnit, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(EmptyUnitSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 12;
            ReportOutput<EmptyUnitSummaryHeaderOutputDto, EmptyUnitSummaryDataOutputDto> emptyUnit = await _emptyUnit.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(emptyUnit, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
