using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-net-sales-detail")]
    public class WaterNetSalesDetailController : BaseController
    {
        private readonly IWaterNetSalesDetailHandler _waterNetSalesDetail;
        public WaterNetSalesDetailController(IWaterNetSalesDetailHandler waterNetSalesDetail)
        {
            _waterNetSalesDetail = waterNetSalesDetail;
            _waterNetSalesDetail.NotNull(nameof(_waterNetSalesDetail));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WaterSalesInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<WaterSalesHeaderOutputDto, WaterNetSalesDetailDataOutputDto> waterSales = await _waterNetSalesDetail.Handle(inputDto, cancellationToken);
            return Ok(waterSales);
        }
    }
}
