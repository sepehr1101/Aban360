using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Queries
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionGetSingleService _cordinalDirectionGetSingleService;
        public CordinalDirectionGetSingleController(
            IUnitOfWork uow,
            ICordinalDirectionGetSingleService cordinalDirectionGetSingleService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _cordinalDirectionGetSingleService = cordinalDirectionGetSingleService;
            _cordinalDirectionGetSingleService.NotNull(nameof(cordinalDirectionGetSingleService));
        }

        [HttpGet, HttpPost]
        [Route("Single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSigle(short id, CancellationToken cancellationToken)
        {
            var cordinalDirection = await _cordinalDirectionGetSingleService.Handle(id, cancellationToken);
            return Ok(cordinalDirection);
        }
    }
}
