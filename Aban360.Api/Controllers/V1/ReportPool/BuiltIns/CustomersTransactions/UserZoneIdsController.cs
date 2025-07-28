using Aban360.Common.Categories.ApiResponse;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserZoneIdsOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw()
        {
            ICollection<UserZoneIdsOutputDto> zoneIds =await UserZoneIdsHandler.Handler(CurrentUser.UserId,(short)ClaimType.DefaultZoneId);
            return Ok(zoneIds);
        }
    }
}
