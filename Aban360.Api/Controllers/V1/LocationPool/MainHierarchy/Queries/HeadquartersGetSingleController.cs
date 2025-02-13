using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/headquarters")]
    public class HeadquartersGetSingleController : BaseController
    {
        private readonly IHeadquarterGetSingleHandler _headquarterGetSingleHandler;
        public HeadquartersGetSingleController(IHeadquarterGetSingleHandler headquarterGetSingleHandler)
        {
            _headquarterGetSingleHandler = headquarterGetSingleHandler;
            _headquarterGetSingleHandler.NotNull(nameof(headquarterGetSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var result = await _headquarterGetSingleHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
