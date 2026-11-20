using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using IBillReturnLookupsService = global::Aban360.OldCalcPool.Application.Features.WaterReturn.Services.IBillReturnLookupsService;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill")]
    public class BillToReturnedGetController : BaseController
    {
        private readonly IBillReturnLookupsService _billReturnLookupsService;
        public BillToReturnedGetController(IBillReturnLookupsService billReturnLookupsService)
        {
            _billReturnLookupsService = billReturnLookupsService;
            _billReturnLookupsService.NotNull(nameof(billReturnLookupsService));
        }

        [HttpPost, HttpGet]
        [Route("return")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BillsCanReturnOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BillsToReturnedGet([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            IEnumerable<BillsCanReturnOutputDto> result = await _billReturnLookupsService.BillToReturnListGetAsync(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
