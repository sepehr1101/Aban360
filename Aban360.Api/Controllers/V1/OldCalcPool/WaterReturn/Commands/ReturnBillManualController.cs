using Aban360.Api.Filters;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Commands
{
    [Route("v1/water-return-manual")]
    public class ReturnBillManualController : BaseController
    {
        private readonly IReturnBillManualHandler _returnBillManualHandler;
        public ReturnBillManualController(IReturnBillManualHandler returnBillManualHandler)
        {
            _returnBillManualHandler = returnBillManualHandler;
            _returnBillManualHandler.NotNull(nameof(returnBillManualHandler));
        }

        [HttpPost]
        [Route("generate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        //[TypeFilter(typeof(EndpointAuthorizationFilter))]
        public async Task<IActionResult> GenerateBill(ReturnBillManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _returnBillManualHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
