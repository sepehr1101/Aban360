using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
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
        private readonly IRegionService _service;
        private readonly ICommonZoneService _commonZoneService;

        public RegionController(IUnitOfWork uow, IRegionService service, ICommonZoneService commonZoneService)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            commonZoneService.NotNull(nameof(commonZoneService));
            _commonZoneService = commonZoneService;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RegionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RegionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RegionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RegionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
        {
            RegionGetDto region = await _service.GetSingleAsync(id, cancellationToken);
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
            ICollection<RegionGetDto> region = await _service.GetAllAsync(cancellationToken);
            return Ok(region);
        }
    }
}
