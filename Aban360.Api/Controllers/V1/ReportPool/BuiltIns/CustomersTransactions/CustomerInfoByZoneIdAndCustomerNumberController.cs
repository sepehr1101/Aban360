using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/customer-info-by-zone-and-customernumber")]
    public class CustomerInfoByZoneIdAndCustomerNumberController : BaseController
    {
        private readonly ICustomerInfoHandler _customerInfoByZoneIdAndCustomerNumber;
        private readonly IReportGenerator _reportGenerator;
        public CustomerInfoByZoneIdAndCustomerNumberController(
            ICustomerInfoHandler customerInfoByZoneIdAndCustomerNumber,
            IReportGenerator reportGenerator)
        {
            _customerInfoByZoneIdAndCustomerNumber = customerInfoByZoneIdAndCustomerNumber;
            _customerInfoByZoneIdAndCustomerNumber.NotNull(nameof(customerInfoByZoneIdAndCustomerNumber));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillIdReppar>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(CustomerInfoByZoneAndCustomerNumberInputDto input, CancellationToken cancellationToken)
        {
            BillIdReppar billIdRepper = await _customerInfoByZoneIdAndCustomerNumber.Handle(input, cancellationToken);
            return Ok(billIdRepper);
        }
    }
}
