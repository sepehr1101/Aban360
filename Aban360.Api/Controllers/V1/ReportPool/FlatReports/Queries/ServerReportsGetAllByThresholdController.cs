using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.FlatReports.Queries
{
    [Route("v1/server-reports")]
    public class ServerReportsGetAllByThresholdController : BaseController
    {
        private readonly IServerReportsGetAllByThresholdHandler _serverReportsGetAllByThresholdHandler;
        private readonly IConfiguration _configuration;
        public ServerReportsGetAllByThresholdController(
            IServerReportsGetAllByThresholdHandler serverReportsGetAllByThresholdHandler,
            IConfiguration configuration)
        {
            _serverReportsGetAllByThresholdHandler = serverReportsGetAllByThresholdHandler;
            _serverReportsGetAllByThresholdHandler.NotNull(nameof(_serverReportsGetAllByThresholdHandler));

            _configuration = configuration;
            _configuration.NotNull(nameof(_configuration));
        }

        [HttpGet]
        [Route("by-user-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServerReportsGetAllDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(CancellationToken cancellationToken)
        {
            int tresholdDay = int.Parse(_configuration["FileManagement:ExcelExpireDay"]);
            IEnumerable<ServerReportsGetAllDto> result = await _serverReportsGetAllByThresholdHandler.Handle(CurrentUser.UserId, tresholdDay+1, cancellationToken);
            return Ok(result);
        }
    }
}
