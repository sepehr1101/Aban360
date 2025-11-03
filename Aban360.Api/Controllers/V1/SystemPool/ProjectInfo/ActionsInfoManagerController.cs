using Aban360.Api.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.ProjectInfo
{
    [Route("project")]
    [AllowAnonymous]
    public class ActionsInfoManagerController : BaseController
    {
        [HttpGet("actions-info")]
        public IActionResult GetActionPath()
        {
            ControllerInspector.ExportControllerActions("ControllersAndActions.csv");
            return Ok(@"Exported successfully to AppData/Excels");
        }
    }
}
