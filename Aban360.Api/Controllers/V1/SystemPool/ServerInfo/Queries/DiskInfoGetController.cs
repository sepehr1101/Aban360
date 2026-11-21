using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using Microsoft.AspNetCore.Mvc;
using Aban360.SystemPool.Application.Features.ServerInfo.Services;

namespace Aban360.Api.Controllers.V1.SystemPool.ServerInfo.Queries
{
    [Route("v1/disk-info")]
    public class DiskInfoGetController : BaseController
    {
        private readonly IServerInfoService _serverInfoService;
        public DiskInfoGetController(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
            _serverInfoService.NotNull(nameof(_serverInfoService));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiskInfo[]>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            DiskInfo[] diskInfo = _serverInfoService.GetDiskInfo();
            return Ok(diskInfo);
        }
    }
}
