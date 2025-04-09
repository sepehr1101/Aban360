using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Contracts;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.ServerInfo.Queries
{
    [Route("v1/disk_info")]
    public class DiskInfoGetController:BaseController
    {
        private readonly IDiskInfoHandler _distInforHandler;
        public DiskInfoGetController(IDiskInfoHandler diskInfoHandler)
        {
            _distInforHandler= diskInfoHandler;
            _distInforHandler.NotNull(nameof(_distInforHandler));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiskInfo[]>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            DiskInfo[] diskInfo= _distInforHandler.Handle();
            return Ok(diskInfo);
        }
    }
}
