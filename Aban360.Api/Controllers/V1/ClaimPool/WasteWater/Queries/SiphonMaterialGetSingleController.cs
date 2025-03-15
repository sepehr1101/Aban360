using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-material")]
    public class SiphonMaterialGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonMaterialGetSingleHandler _siphonMaterialHandler;
        public SiphonMaterialGetSingleController(
            IUnitOfWork uow,
            ISiphonMaterialGetSingleHandler siphonMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonMaterialHandler = siphonMaterialHandler;
            _siphonMaterialHandler.NotNull(nameof(siphonMaterialHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            SiphonMaterialGetDto siphonMaterial = await _siphonMaterialHandler.Handle(id, cancellationToken);
            return Ok(siphonMaterial);
        }
    }
}
