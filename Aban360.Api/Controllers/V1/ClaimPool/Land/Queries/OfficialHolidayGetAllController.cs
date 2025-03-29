using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/official-holiday")]
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
