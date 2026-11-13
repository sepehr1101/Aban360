using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.LocationsInfo
{
    [Route("v1/region")]
    public class RegionDictionaryGetAllController : BaseController
    {
        private readonly IRegionService _service;

        public RegionDictionaryGetAllController(IRegionService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> regions = await _service.GetDictionaryAsync(cancellationToken);
            return Ok(regions);
        }
    }
}
