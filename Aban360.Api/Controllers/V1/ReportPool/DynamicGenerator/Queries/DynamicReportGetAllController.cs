using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator.Queries
{
    [Route("v1/dynamic-report")]
    public class DynamicReportGetAllController : BaseController
    {
        private readonly IDynamicReportGetMastersHandler _dynamicReportGetMastersHandler;
        public DynamicReportGetAllController(IDynamicReportGetMastersHandler tariffConstantGetAllHandler)
        {
            _dynamicReportGetMastersHandler = tariffConstantGetAllHandler;
            _dynamicReportGetMastersHandler.NotNull(nameof(tariffConstantGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DynamicReportMasterDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<DynamicReportMasterDto> dynamicReportMasters = await _dynamicReportGetMastersHandler.Handle(cancellationToken);
            return Ok(dynamicReportMasters);
        }
    }
}
