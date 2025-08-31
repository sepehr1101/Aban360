using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
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
        private readonly IReportGenerator _reportGenerator;
        public LinkServiceStatementController(
            ILinkServiceStatementHandler linkServiceStatementHandler,
            IReportGenerator reportGenerator)
        {
            _linkServiceStatementHandler = linkServiceStatementHandler;
            _linkServiceStatementHandler.NotNull(nameof(linkServiceStatementHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(LinkServiceStatementInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto> linkServiceStatement = await _linkServiceStatementHandler.Handle(input, cancellationToken);
            return Ok(linkServiceStatement);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, LinkServiceStatementInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _linkServiceStatementHandler.Handle, CurrentUser, ReportLiterals.LinkServiceStatement, connectionId);
            return Ok(inputDto);
        }
    }
}
