using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-raw-sales-detail")]
    public class WaterRawSalesDetailController : BaseController
    {
        private readonly IWaterRawSalesDetailHandler _waterRawSalesDetail;
        public WaterRawSalesDetailController(IWaterRawSalesDetailHandler waterRawSalesDetail)
        {
            _waterRawSalesDetail = waterRawSalesDetail;
            _waterRawSalesDetail.NotNull(nameof(_waterRawSalesDetail));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesDetailDataOutputDto> waterSales = await _waterRawSalesDetail.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }
    }
}
