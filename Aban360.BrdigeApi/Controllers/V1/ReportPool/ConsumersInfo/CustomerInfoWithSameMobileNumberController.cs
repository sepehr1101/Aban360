using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerInfoWithSameMobileNumberController : BaseController
    {
        private readonly ICustomerInfoWithSameMobileNumberHandler _customerInfoWithSameMobileNumber;
        public CustomerInfoWithSameMobileNumberController(
            ICustomerInfoWithSameMobileNumberHandler customerInfoWithSameMobileNumber)
        {
            _customerInfoWithSameMobileNumber = customerInfoWithSameMobileNumber;
            _customerInfoWithSameMobileNumber.NotNull(nameof(_customerInfoWithSameMobileNumber));
        }

        [HttpPost, HttpGet]
        [Route("by-mobile/{mobileNumber}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(string mobileNumber, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto> customerInfo = await _customerInfoWithSameMobileNumber.Handle(mobileNumber, cancellationToken);
            return Ok(customerInfo);
        }
    }
}
