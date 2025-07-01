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
        public HandoverDetailController(IHandoverDetailHandler handoverDetail)
        {
            _handoverDetail = handoverDetail;
            _handoverDetail.NotNull(nameof(_handoverDetail));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(HandoverInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto> HandoverDetail = await _handoverDetail.Handle(inputDto, cancellationToken);
            return Ok(HandoverDetail);
        }
    }
}
