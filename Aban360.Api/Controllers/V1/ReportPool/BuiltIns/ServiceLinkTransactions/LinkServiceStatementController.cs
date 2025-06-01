using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/link-service-statement")]
    public class LinkServiceStatementController : BaseController
    {
        private readonly ILinkServiceStatementHandler _linkServiceStatementHandler;
        public LinkServiceStatementController(ILinkServiceStatementHandler linkServiceStatementHandler)
        {
            _linkServiceStatementHandler = linkServiceStatementHandler;
            _linkServiceStatementHandler.NotNull(nameof(linkServiceStatementHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(LinkServiceStatementInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto> linkServiceStatement = await _linkServiceStatementHandler.Handle(input, cancellationToken);
            return Ok(linkServiceStatement);
        }
    }
}
