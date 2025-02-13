using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Queries
{
    [Route("v1/headquarters")]
    public class HeadquartersGetAllController : BaseController
    {
        private readonly IHeadquarterGetAllHandler _headquarterGetAllHandler;
        public HeadquartersGetAllController(IHeadquarterGetAllHandler headquarterGetAllHandler)
        {
            _headquarterGetAllHandler = headquarterGetAllHandler;
            _headquarterGetAllHandler.NotNull(nameof(headquarterGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _headquarterGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
