using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.Sms
{
    [Route("v1/get-customer-mobile")]
    public class GetCustomerMobileController : BaseController
    {
        private readonly IGetCustomerMobileHandler _getCustomerMobileHandler;
        public GetCustomerMobileController(IGetCustomerMobileHandler getCustomerMobileHandler)
        {
            _getCustomerMobileHandler = getCustomerMobileHandler;
            _getCustomerMobileHandler.NotNull(nameof(getCustomerMobileHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GetCustomerMobileOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(GetCustomerMobileInputDto input, CancellationToken cancellationToken)
        {
            GetCustomerMobileOutputDto getCustomerMobile = await _getCustomerMobileHandler.Handle(input, cancellationToken);
            return Ok(getCustomerMobile);
        }
    }
}
