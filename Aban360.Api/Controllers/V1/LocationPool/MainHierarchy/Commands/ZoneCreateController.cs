using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/zone")]
    public class ZoneCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneCreateHandler _zoneCreateHandler;
        public ZoneCreateController(
            IUnitOfWork uow,
            IZoneCreateHandler zoneCreateHandler)
        {
            _uow=uow;
            _uow.NotNull(nameof(uow));

            _zoneCreateHandler = zoneCreateHandler;
            _zoneCreateHandler.NotNull(nameof(zoneCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            await _zoneCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
