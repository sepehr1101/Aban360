using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Queries
{
    [Route("v1/bill-id-tag")]
    public class BillIdTagGetController : BaseController
    {
        private readonly IGetBillIdTagHandler _getHandler;

        public BillIdTagGetController(IGetBillIdTagHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [Route("get/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BillIdTagDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(string billId)
        {
            var tags = await _getHandler.Handle(billId);
            return Ok(tags);
        }
    }
}
