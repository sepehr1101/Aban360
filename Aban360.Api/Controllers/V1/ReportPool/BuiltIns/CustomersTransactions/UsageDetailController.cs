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
    [Route("v1/usage-detail")]
    public class UsageDetailController : BaseController
    {
        private readonly IUsageDetailHandler _UsageDetail;
        private readonly IReportGenerator _reportGenerator;
        public UsageDetailController(
            IUsageDetailHandler UsageDetail,
            IReportGenerator reportGenerator)
        {
            _UsageDetail = UsageDetail;
            _UsageDetail.NotNull(nameof(_UsageDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageDetailHeaderOutputDto, UsageDetailDataOutputDto> usageDetail = await _UsageDetail.Handle(inputDto, cancellationToken);
            return Ok(usageDetail);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UsageDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _UsageDetail.Handle, CurrentUser, ReportLiterals.UsageDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
