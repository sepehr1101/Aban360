using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-type")]
    public class SiphonTypeGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonTypeGetSingleHandler _siphonTypeHandler;
        public SiphonTypeGetSingleController(
            IUnitOfWork uow,
            ISiphonTypeGetSingleHandler siphonTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonTypeHandler = siphonTypeHandler;
            _siphonTypeHandler.NotNull(nameof(siphonTypeHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            SiphonTypeGetDto siphonType = await _siphonTypeHandler.Handle(id, cancellationToken);
            return Ok(siphonType);
        }
    }
}
