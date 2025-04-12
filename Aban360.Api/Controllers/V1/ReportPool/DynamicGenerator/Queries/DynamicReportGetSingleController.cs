using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator.Queries
{
    [Route("v1/dynamic-Report")]
    public class DynamicReportGetSingleController : BaseController
    {
        private readonly IDynamicReportGetSingleHandler _dynamicReportGetSingleHandler;
        public DynamicReportGetSingleController(IDynamicReportGetSingleHandler tariffConstantGetSingleHandler)
        {
            _dynamicReportGetSingleHandler = tariffConstantGetSingleHandler;
            _dynamicReportGetSingleHandler.NotNull(nameof(tariffConstantGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DynamicReportGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var dynamicReports = await _dynamicReportGetSingleHandler.Handle(id, cancellationToken);
            return Ok(dynamicReports);
        }
    }
}
