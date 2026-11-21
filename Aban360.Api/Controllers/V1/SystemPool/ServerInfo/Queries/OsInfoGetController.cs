using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using Microsoft.AspNetCore.Mvc;
using Aban360.SystemPool.Application.Features.ServerInfo.Services;

namespace Aban360.Api.Controllers.V1.SystemPool.ServerInfo.Queries
{
    [Route("v1/os-info")]
    public class OsInfoGetController : BaseController
    {
        private readonly IServerInfoService _serverInfoService;
        public OsInfoGetController(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
            _serverInfoService.NotNull(nameof(_serverInfoService));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OsInfo>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            OsInfo osInfo = _serverInfoService.GetOsInfo();
            return Ok(osInfo);
        }
    }
}
