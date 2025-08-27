using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/usage-summary")]
    public class UsageSummaryController : BaseController
    {
        private readonly IUsageSummaryHandler _UsageSummary;
        private readonly IReportGenerator _reportGenerator;
        public UsageSummaryController(
            IUsageSummaryHandler UsageSummary,
            IReportGenerator reportGenerator)
        {
            _UsageSummary = UsageSummary;
            _UsageSummary.NotNull(nameof(_UsageSummary));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto> usageSummary = await _UsageSummary.Handle(inputDto, cancellationToken);
            return Ok(usageSummary);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UsageSummaryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _UsageSummary.Handle, CurrentUser, ReportLiterals.UsageSummary, connectionId);
            return Ok(inputDto);
        }
    }
}
