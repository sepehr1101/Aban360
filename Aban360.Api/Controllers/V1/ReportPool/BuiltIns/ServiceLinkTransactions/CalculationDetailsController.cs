using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/calculation-details")]
    public class CalculationDetailsController : BaseController
    {
        private readonly ICalculationDetailsHandler _calculationDetailsHandler;
        public CalculationDetailsController(ICalculationDetailsHandler calculationDetailsHandler)
        {
            _calculationDetailsHandler = calculationDetailsHandler;
            _calculationDetailsHandler.NotNull(nameof(calculationDetailsHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CalculationDetailsHeaderOutputDto, CalculationDetailsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(CalculationDetailsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CalculationDetailsHeaderOutputDto, CalculationDetailsDataOutputDto> calculationDetails = await _calculationDetailsHandler.Handle(input, cancellationToken);
            return Ok(calculationDetails);
        }
    }
}
