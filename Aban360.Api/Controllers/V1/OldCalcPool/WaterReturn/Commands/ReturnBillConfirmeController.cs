using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.WaterReturn.Commands
{
    [Route("v1/water-return")]
    public class ReturnBillConfirmeController : BaseController
    {
        private readonly IReturnBillConfirmeHandler _returnConfirmHandler;
        public ReturnBillConfirmeController(IReturnBillConfirmeHandler returnConfirmHandler)
        {
            _returnConfirmHandler = returnConfirmHandler;
            _returnConfirmHandler.NotNull(nameof(returnConfirmHandler));
        }

        [HttpPost, HttpGet]
        [Route("confirme")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReturnBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirme([FromBody] ReturnBillConfirmeByBillIdInputDto input, CancellationToken cancellationToken)
        {
            RepairCreateDto result = await _returnConfirmHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}