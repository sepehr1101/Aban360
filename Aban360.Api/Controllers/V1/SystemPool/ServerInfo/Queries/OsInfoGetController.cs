using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Contracts;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.ServerInfo.Queries
{
    [Route("v1/os_info")]
    public class OsInfoGetController:BaseController
    {
        private readonly IOsInfoHandler _osInforHandler;
        public OsInfoGetController(IOsInfoHandler osInforHandler)
        {
            _osInforHandler= osInforHandler;
            _osInforHandler.NotNull(nameof(_osInforHandler));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OsInfo>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            OsInfo osInfo= _osInforHandler.Handle();
            return Ok(osInfo);
        }
    }
}
