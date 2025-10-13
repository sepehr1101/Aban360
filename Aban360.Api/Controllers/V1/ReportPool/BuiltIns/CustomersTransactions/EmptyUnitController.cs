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
    [Route("v1/empty-unit")]
    public class EmptyUnitController : BaseController
    {
        private readonly IEmptyUnitHandler _emptyUnit;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitController(
            IEmptyUnitHandler emptyUnit,
            IReportGenerator reportGenerator)
        {
            _emptyUnit = emptyUnit;
            _emptyUnit.NotNull(nameof(_emptyUnit));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnit.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId,EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken,_emptyUnit.Handle,CurrentUser,ReportLiterals.EmptyUnit,connectionId);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 10;
            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnit.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(emptyUnit, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
