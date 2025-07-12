using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/agent-warnings")]
    public class AgentWarningsController : BaseController
    {
        private readonly IAgentWarningsHandler _agentWarnings;
        public AgentWarningsController(IAgentWarningsHandler agentWarnings)
        {
            _agentWarnings = agentWarnings;
            _agentWarnings.NotNull(nameof(_agentWarnings));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(AgentWarningsInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto> AgentWarnings = await _agentWarnings.Handle(inputDto, cancellationToken);
            return Ok(AgentWarnings);
        }
    }
}
