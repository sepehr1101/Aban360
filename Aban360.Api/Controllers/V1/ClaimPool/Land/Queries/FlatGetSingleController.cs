using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/falt")]
    public class FlatGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatGetSingleHandler _flatHandler;
        public FlatGetSingleController(
            IUnitOfWork uow,
            IFlatGetSingleHandler flatHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flatHandler = flatHandler;
            _flatHandler.NotNull(nameof(flatHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            FlatGetDto flat = await _flatHandler.Handle(id, cancellationToken);
            return Ok(flat);
        }
    }
}
