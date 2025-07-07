using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link-calculation-details")]
    public class ServiceLinkCalculationDetailsController : BaseController
    {
        private readonly IServiceLinkCalculationDetailsHandler _calculationDetailsHandler;
        public ServiceLinkCalculationDetailsController(IServiceLinkCalculationDetailsHandler calculationDetailsHandler)
        {
            _calculationDetailsHandler = calculationDetailsHandler;
            _calculationDetailsHandler.NotNull(nameof(calculationDetailsHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkCalculationDetailsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto> calculationDetails = await _calculationDetailsHandler.Handle(input, cancellationToken);
            return Ok(calculationDetails);
        }
    }
}
