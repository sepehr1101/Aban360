using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/service-link-debtor-customers")]
    public class ServiceLinkDebtorCustomersController : BaseController
    {
        private readonly IServiceLinkDebtorCustomersHandler _serviceLinkDebtorCustomersHandler;
        public ServiceLinkDebtorCustomersController(IServiceLinkDebtorCustomersHandler serviceLinkDebtorCustomersHandler)
        {
            _serviceLinkDebtorCustomersHandler = serviceLinkDebtorCustomersHandler;
            _serviceLinkDebtorCustomersHandler.NotNull(nameof(_serviceLinkDebtorCustomersHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkDebtorCustomersHeaderOutputDto, ServiceLinkDebtorCustomersDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkDebtorCustomersInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkDebtorCustomersHeaderOutputDto, ServiceLinkDebtorCustomersDataOutputDto> ServiceLinkDebtorCustomers = await _serviceLinkDebtorCustomersHandler.Handle(inputDto, cancellationToken);
            return Ok(ServiceLinkDebtorCustomers);
        }
    }
}
