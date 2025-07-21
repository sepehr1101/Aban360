using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/zone")]
    public class ZoneGetAllController : BaseController
    {
        private readonly IZoneGetAllHandler _zoneGetAllHandler;
        private readonly IZoneAllHandler _zoneAllHandler;

        public ZoneGetAllController(
            IZoneGetAllHandler zoneGetAllHandler,
            IZoneAllHandler zoneAllHandler)
        {
            _zoneGetAllHandler = zoneGetAllHandler;
            _zoneGetAllHandler.NotNull(nameof(zoneGetAllHandler));

            _zoneAllHandler=zoneAllHandler;
            _zoneAllHandler.NotNull(nameof(zoneAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ZoneGetDto> zone = await _zoneGetAllHandler.Handle(cancellationToken);
            return Ok(zone);
        }
        
        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserZoneIdsOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPrinciple(CancellationToken cancellationToken)
        {
            ICollection<UserZoneIdsOutputDto> zone =( await _zoneAllHandler.Handle(cancellationToken)).ToList();
            return Ok(zone);
        }
    }
}
