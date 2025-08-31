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
    [Route("v1/usage")]
    public class UsageController : BaseController
    {
        private readonly IUsageHandler _usage;
        private readonly IReportGenerator _reportGenerator;
        public UsageController(
            IUsageHandler usage,
            IReportGenerator reportGenerator)
        {
            _usage = usage;
            _usage.NotNull(nameof(_usage));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto> usage = await _usage.Handle(inputDto, cancellationToken);
            return Ok(usage);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UsageInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _usage.Handle, CurrentUser, ReportLiterals.Usage, connectionId);
            return Ok(inputDto);
        }
    }
}
