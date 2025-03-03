using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/estate-bound-type")]
    public class EstateBoundTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateBoundTypeGetSingleHandler _getSingleHandler;
        public EstateBoundTypeGetSingleController(
            IUnitOfWork uow,
            IEstateBoundTypeGetSingleHandler getSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getSingleHandler = getSingleHandler;
            _getSingleHandler.NotNull(nameof(_getSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateBoundTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var estateBoundType = await _getSingleHandler.Handle(id, cancellationToken);
            return Ok(estateBoundType);
        }
    }
}
