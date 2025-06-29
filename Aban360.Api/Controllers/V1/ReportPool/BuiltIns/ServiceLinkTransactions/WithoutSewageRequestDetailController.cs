using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/without-sewage-request-detail")]
    public class WithoutSewageRequestDetailController : BaseController
    {
        private readonly IWithoutSewageRequestDetailHandler _withoutSewageRequestDetailHandler;
        public WithoutSewageRequestDetailController(IWithoutSewageRequestDetailHandler withoutSewageRequestDetailHandler)
        {
            _withoutSewageRequestDetailHandler = withoutSewageRequestDetailHandler;
            _withoutSewageRequestDetailHandler.NotNull(nameof(withoutSewageRequestDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutSewageRequestInputDto input,CancellationToken cancellationToken)
        {
            var result=await _withoutSewageRequestDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
