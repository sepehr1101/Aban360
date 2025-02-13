using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/region")]
    public class RegionDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRegionDeleteHandler _regionDeleteHandler;
        public RegionDeleteController(
            IUnitOfWork uow,
            IRegionDeleteHandler regionDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(regionDeleteHandler));

            _regionDeleteHandler = regionDeleteHandler;
            _regionDeleteHandler.NotNull(nameof(regionDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RegionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _regionDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
