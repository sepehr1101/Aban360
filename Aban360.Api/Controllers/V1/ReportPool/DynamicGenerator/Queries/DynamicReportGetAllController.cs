using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator.Queries
{
    [Route("v1/dynamic-Report")]
    public class DynamicReportGetAllController : BaseController
    {
        private readonly IDynamicReportGetAllHandler _dynamicReportGetAllHandler;
        public DynamicReportGetAllController(IDynamicReportGetAllHandler tariffConstantGetAllHandler)
        {
            _dynamicReportGetAllHandler = tariffConstantGetAllHandler;
            _dynamicReportGetAllHandler.NotNull(nameof(tariffConstantGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DynamicReportGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var dynamicReport = await _dynamicReportGetAllHandler.Handle(cancellationToken);
            return Ok(dynamicReport);
        }
    }
}
