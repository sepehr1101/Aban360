using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/prepayment-and-calculation")]
    public class PrepaymentAndCalculationController:BaseController
    {
        private readonly IPrepaymentAndCalculationHandler _prepaymentAndCalculationHandler;
        public PrepaymentAndCalculationController(IPrepaymentAndCalculationHandler prepaymentAndCalculationHandler)
        {
            _prepaymentAndCalculationHandler = prepaymentAndCalculationHandler;
            _prepaymentAndCalculationHandler.NotNull(nameof(_prepaymentAndCalculationHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIfo(PrepaymentAndCalculationInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto> prepaymentAndCalculation =await _prepaymentAndCalculationHandler.Handle(inputDto,cancellationToken);
            return Ok(prepaymentAndCalculation);
        }
    }
}
