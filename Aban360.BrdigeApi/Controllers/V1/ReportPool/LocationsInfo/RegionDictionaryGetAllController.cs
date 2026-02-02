using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.LocationsInfo
{
    [Route("v1/region")]
    public class RegionDictionaryGetAllController : BaseController
    {
        private readonly IRegionDictionaryGetAllHandler _regionDictionaryGetAllHandler;
        public RegionDictionaryGetAllController(IRegionDictionaryGetAllHandler regionDictionaryGetAllHandler)
        {
            _regionDictionaryGetAllHandler = regionDictionaryGetAllHandler;
            _regionDictionaryGetAllHandler.NotNull(nameof(regionDictionaryGetAllHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> regions = await _regionDictionaryGetAllHandler.Handle(cancellationToken);
            return Ok(regions);
        }
    }
}
