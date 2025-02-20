using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon-material")]
    public class SiphonMaterialUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonMaterialUpdateHandler _siphonMaterialHandler;
        public SiphonMaterialUpdateController(
            IUnitOfWork uow,
            ISiphonMaterialUpdateHandler siphonMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonMaterialHandler = siphonMaterialHandler;
            _siphonMaterialHandler.NotNull(nameof(siphonMaterialHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SiphonMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _siphonMaterialHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
