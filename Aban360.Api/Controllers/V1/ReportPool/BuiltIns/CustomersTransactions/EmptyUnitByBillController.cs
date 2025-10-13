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
    [Route("v1/empty-unit-by-bill")]
    public class EmptyUnitByBillController : BaseController
    {
        private readonly IEmptyUnitByBillHandler _emptyUnitByBill;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitByBillController(
            IEmptyUnitByBillHandler emptyUnitByBill,
            IReportGenerator reportGenerator)
        {
            _emptyUnitByBill = emptyUnitByBill;
            _emptyUnitByBill.NotNull(nameof(_emptyUnitByBill));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnitByBill.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnitByBill.Handle, CurrentUser, ReportLiterals.EmptyUnit, connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 20;
            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnitByBill.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(emptyUnit, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
