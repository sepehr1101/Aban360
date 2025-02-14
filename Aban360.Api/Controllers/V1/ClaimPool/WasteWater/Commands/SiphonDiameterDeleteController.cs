using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon-diameter")]
    public class SiphonDiameterDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonDiameterDeleteHandler _siphonDiameterHandler;
        public SiphonDiameterDeleteController(
            IUnitOfWork uow,
            ISiphonDiameterDeleteHandler siphonDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonDiameterHandler = siphonDiameterHandler;
            _siphonDiameterHandler.NotNull(nameof(siphonDiameterHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] SiphonDiameterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _siphonDiameterHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
