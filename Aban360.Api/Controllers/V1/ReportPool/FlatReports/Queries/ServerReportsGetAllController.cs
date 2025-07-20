using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.FlatReports.Queries
{
    [Route("v1/server-reports")]
    public class ServerReportsGetAllController : BaseController
    {
        private readonly IServerReportsGetAllHandler _serverReportsGetAllHandler;
        public ServerReportsGetAllController(IServerReportsGetAllHandler contractualCapacity)
        {
            _serverReportsGetAllHandler = contractualCapacity;
            _serverReportsGetAllHandler.NotNull(nameof(_serverReportsGetAllHandler));
        }

        [ HttpGet]
        [Route("by-user-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServerReportsGetAllDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(CancellationToken cancellationToken)
        {
            var result = await _serverReportsGetAllHandler.Handle(CurrentUser.UserId,cancellationToken);
            return Ok(result);
        }
    }
}
