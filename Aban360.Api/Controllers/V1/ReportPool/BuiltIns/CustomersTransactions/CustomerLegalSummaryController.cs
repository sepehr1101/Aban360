using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/customer")]
    public class CustomerLegalSummaryController : BaseController
    {
        private readonly ICustomerLegalSummaryHandler _summaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public CustomerLegalSummaryController(
            ICustomerLegalSummaryHandler summaryHandler,
            IReportGenerator reportGenerator)
        {
            _summaryHandler = summaryHandler;
            _summaryHandler.NotNull(nameof(summaryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("legal-summary-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SummaryRaw([FromBody] CustomerLegalInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryDataOutputDto> result = await _summaryHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("legal-summary-excel/{connectionId}")]
        public async Task<IActionResult> SummaryExcel(string connectionId, [FromBody] CustomerLegalInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryDataOutputDto> result = await _summaryHandler.Handle(input, cancellationToken);
            await _reportGenerator.FireAndInform(input, cancellationToken, _summaryHandler.Handle, CurrentUser, ReportLiterals.CustomerLegalSummary, connectionId);
            return Ok(input);
        }
    }
}
