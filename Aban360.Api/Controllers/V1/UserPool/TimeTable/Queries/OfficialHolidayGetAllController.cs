using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Queries
{
    [Route("v4/official-holiday")]
    public class OfficialHolidayGetAllController : BaseController
    {
        private readonly IOfficialHolidayGetAllHandler officialHolidayGetAllHandler;
        public OfficialHolidayGetAllController(IOfficialHolidayGetAllHandler officialHolidayGetAllHandler)
        {
            this.officialHolidayGetAllHandler = officialHolidayGetAllHandler;
            this.officialHolidayGetAllHandler.NotNull(nameof(officialHolidayGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfficialHolidayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var officialHolidays = await officialHolidayGetAllHandler.Handle(cancellationToken);
            return Ok(officialHolidays);
        }
    }
}
