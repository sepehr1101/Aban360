using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/financial-statement-total")]
    public class FinancialStatementTotalController : BaseController
    {
        private readonly IFinancialStatementTotalHandler _financialStatementHandler;
        private readonly IReportGenerator _reportGenerator;
        public FinancialStatementTotalController(
            IFinancialStatementTotalHandler financialStatementHandler,
            IReportGenerator reportGenerator)
        {
            _financialStatementHandler = financialStatementHandler;
            _financialStatementHandler.NotNull(nameof(_financialStatementHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<FinancialStatementHeaderOutputDto, FinancialStatementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(FinancialStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<FinancialStatementHeaderOutputDto, FinancialStatementDataOutputDto> FinancialStatement = await _financialStatementHandler.Handle(inputDto, cancellationToken);
            return Ok(FinancialStatement);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, FinancialStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _financialStatementHandler.Handle, CurrentUser, ReportLiterals.FinancialStatementTotal, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(FinancialStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = (int)StiReportCodeLiterals.FinancialStatementWaterTotal;
            ReportOutput<FinancialStatementHeaderOutputDto, FinancialStatementDataOutputDto> result = await _financialStatementHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
