using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill")]
    public class BillInstallmentController : BaseController
    {
        public BillInstallmentController()
        {
        }

        [HttpPost]
        [Route("installment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillInstallmentInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCanRemoved([FromBody] BillInstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            //todo: calcInstallment
            return Ok(inputDto);
        }
    }
}
