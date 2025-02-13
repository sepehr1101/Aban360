using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionGetAllHandler _cordinalDirectionGetAllHandler;
        public CordinalDirectionGetAllController(
            IUnitOfWork uow,
            ICordinalDirectionGetAllHandler cordinalDirectionGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _cordinalDirectionGetAllHandler = cordinalDirectionGetAllHandler;
            _cordinalDirectionGetAllHandler.NotNull(nameof(cordinalDirectionGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("All")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CordinalDirectionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
        {
            var cordinalDirection = await _cordinalDirectionGetAllHandler.Handle(cancellationToken);
            return Ok(cordinalDirection);
        }
    }
}
