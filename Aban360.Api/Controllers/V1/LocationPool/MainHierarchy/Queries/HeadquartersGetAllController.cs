using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HeadquarterGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<HeadquarterGetDto> result = await _headquarterGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
