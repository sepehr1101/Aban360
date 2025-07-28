using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/handover-detail")]
    public class HandoverDetailController : BaseController
    {
        private readonly IHandoverDetailHandler _handoverDetail;
        private readonly IReportGenerator _reportGenerator;
        public HandoverDetailController(
            IHandoverDetailHandler handoverDetail,
            IReportGenerator reportGenerator)
        {
            _handoverDetail = handoverDetail;
            _handoverDetail.NotNull(nameof(_handoverDetail));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(HandoverInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto> HandoverDetail = await _handoverDetail.Handle(inputDto, cancellationToken);
            return Ok(HandoverDetail);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, HandoverInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _handoverDetail.Handle, CurrentUser, ReportLiterals.HandoverDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
