using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Db70.Services;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/bill")]
    public class BillReturnCauseGetAllController : BaseController
    {
        private readonly IBillReturnCauseService _billReturnCauseService;
        public BillReturnCauseGetAllController(IBillReturnCauseService billReturnCauseService)
        {
            _billReturnCauseService = billReturnCauseService;
            _billReturnCauseService.NotNull(nameof(billReturnCauseService));
        }

        [HttpGet]
        [Route("return-cause")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _billReturnCauseService.GetAllDictionaryAsync(cancellationToken);
            return Ok(result);
        }
    }
}
