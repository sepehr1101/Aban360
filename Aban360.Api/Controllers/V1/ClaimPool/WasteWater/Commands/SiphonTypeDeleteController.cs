using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon-type")]
    public class SiphonTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonTypeDeleteHandler _siphonTypeHandler;
        public SiphonTypeDeleteController(
            IUnitOfWork uow,
            ISiphonTypeDeleteHandler siphonTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonTypeHandler = siphonTypeHandler;
            _siphonTypeHandler.NotNull(nameof(siphonTypeHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SiphonTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonTypeHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
