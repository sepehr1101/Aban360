using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/zone")]
    public class ZoneUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneUpdateHandler _zoneUpdateHandler;
        public ZoneUpdateController(
            IUnitOfWork uow,
            IZoneUpdateHandler zoneUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _zoneUpdateHandler = zoneUpdateHandler;
            _zoneUpdateHandler.NotNull(nameof(zoneUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _zoneUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
