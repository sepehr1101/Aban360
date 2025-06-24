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
        public UsageController(IUsageHandler usage)
        {
            _usage = usage;
            _usage.NotNull(nameof(_usage));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto> usage = await _usage.Handle(inputDto, cancellationToken);
            return Ok(usage);
        }
    }
}
