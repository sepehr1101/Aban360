using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon-type")]
    public class SiphonTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonTypeUpdateHandler _siphonTypeHandler;
        public SiphonTypeUpdateController(
            IUnitOfWork uow,
            ISiphonTypeUpdateHandler siphonTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonTypeHandler = siphonTypeHandler;
            _siphonTypeHandler.NotNull(nameof(siphonTypeHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SiphonTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _siphonTypeHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
