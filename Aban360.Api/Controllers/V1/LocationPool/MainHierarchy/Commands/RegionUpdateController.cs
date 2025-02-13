using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/region")]
    public class RegionUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRegionUpdateHandler _regionUpdateHandler;
        public RegionUpdateController(
            IUnitOfWork uow,
            IRegionUpdateHandler regionUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(regionUpdateHandler));

            _regionUpdateHandler = regionUpdateHandler;
            _regionUpdateHandler.NotNull(nameof(regionUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RegionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _regionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
