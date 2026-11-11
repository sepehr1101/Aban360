using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/region")]
    public class RegionController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRegionCreateHandler _regionCreateHandler;
        private readonly IRegionUpdateHandler _regionUpdateHandler;
        private readonly IRegionDeleteHandler _regionDeleteHandler;
        private readonly IRegionGetSingleHandler _regionGetSingleHandler;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IRegionGetAllHandler _regionGetAllHandler;

        public RegionController(
            IUnitOfWork uow,
            IRegionCreateHandler regionCreateHandler,
            IRegionUpdateHandler regionUpdateHandler,
            IRegionDeleteHandler regionDeleteHandler,
            IRegionGetSingleHandler regionGetSingleHandler,
            ICommonZoneService commonZoneService,
            IRegionGetAllHandler regionGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(regionCreateHandler));
            _uow.NotNull(nameof(regionUpdateHandler));
            _uow.NotNull(nameof(regionDeleteHandler));

            _regionCreateHandler = regionCreateHandler;
            _regionCreateHandler.NotNull(nameof(regionCreateHandler));

            _regionUpdateHandler = regionUpdateHandler;
            _regionUpdateHandler.NotNull(nameof(regionUpdateHandler));

            _regionDeleteHandler = regionDeleteHandler;
            _regionDeleteHandler.NotNull(nameof(regionDeleteHandler));

            _regionGetSingleHandler = regionGetSingleHandler;
            _regionGetSingleHandler.NotNull(nameof(regionGetSingleHandler));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _regionGetAllHandler = regionGetAllHandler;
            _regionGetAllHandler.NotNull(nameof(regionGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RegionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _regionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
        {
            RegionGetDto region = await _regionGetSingleHandler.Handle(id, cancellationToken);
            return Ok(region);
        }

        [HttpGet]
        [Route("my-default")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyDefaultZone(CancellationToken cancellationToken)
        {
            NumericDictionary zone = await _commonZoneService.GetDefaultRegion(CurrentUser);
            return Ok(zone);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RegionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<RegionGetDto> region = await _regionGetAllHandler.Handle(cancellationToken);
            return Ok(region);
        }
    }
}
