using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-material")]
    public class SiphonMaterialGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonMaterialGetAllHandler _siphonMaterialHandler;
        public SiphonMaterialGetAllController(
            IUnitOfWork uow,
            ISiphonMaterialGetAllHandler siphonMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonMaterialHandler = siphonMaterialHandler;
            _siphonMaterialHandler.NotNull(nameof(siphonMaterialHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SiphonMaterialGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SiphonMaterialGetDto> siphonMaterials = await _siphonMaterialHandler.Handle(cancellationToken);
            return Ok(siphonMaterials);
        }
    }
}
