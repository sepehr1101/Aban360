using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link")]
    public class ServiceLinkSetPayController : BaseController
    {
        private readonly IServiceLinkRegisterManualHandler _serviceLinkRegisterHandler;
        public ServiceLinkSetPayController(IServiceLinkRegisterManualHandler serviceLinkRegisterHandler)
        {
            _serviceLinkRegisterHandler = serviceLinkRegisterHandler;
            _serviceLinkRegisterHandler.NotNull(nameof(serviceLinkRegisterHandler));
        }

        [HttpPost, HttpGet]
        [Route("set-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkRegisterManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPay(ServiceLinkRegisterManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkRegisterHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
