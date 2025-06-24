using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/usage-detail")]
    public class UsageDetailController : BaseController
    {
        private readonly IUsageDetailHandler _UsageDetail;
        public UsageDetailController(IUsageDetailHandler UsageDetail)
        {
            _UsageDetail = UsageDetail;
            _UsageDetail.NotNull(nameof(_UsageDetail));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto> usageDetail = await _UsageDetail.Handle(inputDto, cancellationToken);
            return Ok(usageDetail);
        }
    }
}
