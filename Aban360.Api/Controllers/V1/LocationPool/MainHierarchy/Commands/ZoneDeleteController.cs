using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/zone")]
    public class ZoneDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneDeleteHandler _zoneDeleteHandler;
        public ZoneDeleteController(
            IUnitOfWork uow,
            IZoneDeleteHandler zoneDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _zoneDeleteHandler = zoneDeleteHandler;
            _zoneDeleteHandler.NotNull(nameof(zoneDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _zoneDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
