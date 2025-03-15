using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon")]
    public class SiphonDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonDeleteHandler _siphonHandler;
        public SiphonDeleteController(
            IUnitOfWork uow,
            ISiphonDeleteHandler siphonHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonHandler = siphonHandler;
            _siphonHandler.NotNull(nameof(siphonHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
