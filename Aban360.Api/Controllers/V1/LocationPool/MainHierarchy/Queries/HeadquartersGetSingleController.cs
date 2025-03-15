using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<HeadquarterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            HeadquarterGetDto result = await _headquarterGetSingleHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
