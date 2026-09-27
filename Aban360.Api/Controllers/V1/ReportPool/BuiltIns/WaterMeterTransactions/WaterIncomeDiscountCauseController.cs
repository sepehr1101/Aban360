using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/water-income-discount-cause")]
    public class WaterIncomeDiscountCauseController : BaseController
    {
        private readonly IWaterIncomeDiscountCauseGetHandler _waterDiscountCauseHandler;
        public WaterIncomeDiscountCauseController(IWaterIncomeDiscountCauseGetHandler waterDiscountCauseHandler)
        {
            _waterDiscountCauseHandler = waterDiscountCauseHandler;
            _waterDiscountCauseHandler.NotNull(nameof(waterDiscountCauseHandler));
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult Get(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> discountCauseList = _waterDiscountCauseHandler.Handle(cancellationToken);
            return Ok(discountCauseList);
        }
    }
}
