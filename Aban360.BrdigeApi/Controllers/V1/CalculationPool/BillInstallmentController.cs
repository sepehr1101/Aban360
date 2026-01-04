using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/bill")]
    public class BillInstallmentController : BaseController
    {
        private readonly IBillInstallmentHandler _billInstallmentHandler;
        public BillInstallmentController(IBillInstallmentHandler billInstallmentHandler)
        {
            _billInstallmentHandler = billInstallmentHandler;
            _billInstallmentHandler.NotNull(nameof(billInstallmentHandler));
        }

        [HttpPost]
        [Route("installment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Installment([FromBody] InstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto> result = await _billInstallmentHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
