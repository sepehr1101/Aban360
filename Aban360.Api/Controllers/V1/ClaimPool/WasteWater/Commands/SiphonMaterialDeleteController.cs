using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon-material")]
    public class SiphonMaterialDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonMaterialDeleteHandler _siphonMaterialHandler;
        public SiphonMaterialDeleteController(
            IUnitOfWork uow,
            ISiphonMaterialDeleteHandler siphonMaterialHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonMaterialHandler = siphonMaterialHandler;
            _siphonMaterialHandler.NotNull(nameof(siphonMaterialHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonMaterialDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonMaterialHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
