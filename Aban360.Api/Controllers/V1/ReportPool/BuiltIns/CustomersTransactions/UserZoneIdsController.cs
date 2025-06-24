using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations;
using Aban360.UserPool.Persistence.Constants.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/user-zone-ids")]
    public class UserZoneIdsController:BaseController
    {
        public UserZoneIdsController()
        {}

        [HttpGet, HttpPost]
        [Route("raw")]
        public async Task<IActionResult> GetRaw()
        {
            List<int> zoneIds=await UserZoneIdsHandler.Handler(CurrentUser.UserId,(short)ClaimType.ZoneId);
            return Ok(zoneIds);
        }
    }
}
