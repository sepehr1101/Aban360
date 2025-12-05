using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/zone")]
    public class ZoneGetSingleController : BaseController
    {
        private readonly IZoneGetSingleHandler _zoneGetSingleHandler;
        private readonly ICommonZoneService _zoneCommonService;
        public ZoneGetSingleController(
            IZoneGetSingleHandler zoneGetSingleHandler,
            ICommonZoneService commonZoneService)
        {
            _zoneGetSingleHandler = zoneGetSingleHandler;
            _zoneGetSingleHandler.NotNull(nameof(zoneGetSingleHandler));

            _zoneCommonService = commonZoneService;
            _zoneCommonService.NotNull(nameof(_zoneCommonService));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id,CancellationToken cancellationToken)
        {
            ZoneGetDto zone = await _zoneGetSingleHandler.Handle(id,cancellationToken);
            return Ok(zone);
        }

        [HttpGet]
        [Route("my-default")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyDefaultZone(CancellationToken cancellationToken)
        {
            NumericDictionary zone = await _zoneCommonService.GetDefault(CurrentUser);
            return Ok(zone);
        }
    }
}
