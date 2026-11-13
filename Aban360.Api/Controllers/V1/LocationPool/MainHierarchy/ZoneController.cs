using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/zone")]
    public class ZoneController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneService _service;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IZoneAllHandler _zoneAllHandler;
        public ZoneController(IUnitOfWork uow, IZoneService service, ICommonZoneService commonZoneService, IZoneAllHandler zoneAllHandler)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            commonZoneService.NotNull(nameof(commonZoneService));
            _commonZoneService = commonZoneService;
            zoneAllHandler.NotNull(nameof(zoneAllHandler));
            _zoneAllHandler = zoneAllHandler;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id,CancellationToken cancellationToken)
        {
            ZoneGetDto zone = await _service.GetSingleAsync(id,cancellationToken);
            return Ok(zone);
        }

        [HttpGet]
        [Route("my-default")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyDefaultZone(CancellationToken cancellationToken)
        {
            NumericDictionary zone = await _commonZoneService.GetDefault(CurrentUser);
            return Ok(zone);
        }

        [HttpGet, HttpPost]
        [Route("all-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ZoneGetDto> zone = await _service.GetAllAsync(cancellationToken);
            return Ok(zone);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserZoneIdsOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPrinciple(CancellationToken cancellationToken)
        {
            ICollection<UserZoneIdsOutputDto> zone = await _zoneAllHandler.Handle(CurrentUser, cancellationToken);
            return Ok(zone);
        }
    }
}
