using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/region")]
    public class RegionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRegionCreateHandler _regionCreateHandler;
        public RegionCreateController(
            IUnitOfWork uow,
            IRegionCreateHandler regionCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(regionCreateHandler));

            _regionCreateHandler = regionCreateHandler;
            _regionCreateHandler.NotNull(nameof(regionCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RegionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _regionCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
