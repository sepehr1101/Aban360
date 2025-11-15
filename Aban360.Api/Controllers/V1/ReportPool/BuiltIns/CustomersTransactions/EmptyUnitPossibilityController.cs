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
    [Route("v1/empty-unit-possibility")]
    public class EmptyUnitPossibilityController : BaseController
    {
        private readonly IEmptyUnitPossibilityHandler _emptyUnitPossibility;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitPossibilityController(
            IEmptyUnitPossibilityHandler emptyUnitPossibility,
            IReportGenerator reportGenerator)
        {
            _emptyUnitPossibility = emptyUnitPossibility;
            _emptyUnitPossibility.NotNull(nameof(_emptyUnitPossibility));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitPossibilityHeaderOutputDto, EmptyUnitPossibilityDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitPossibilityInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitPossibilityHeaderOutputDto, EmptyUnitPossibilityDataOutputDto> emptyUnit = await _emptyUnitPossibility.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitPossibilityInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnitPossibility.Handle, CurrentUser, ReportLiterals.EmptyUnit, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(EmptyUnitPossibilityInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 680;
            ReportOutput<EmptyUnitPossibilityHeaderOutputDto, EmptyUnitPossibilityDataOutputDto> emptyUnit = await _emptyUnitPossibility.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(emptyUnit, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
