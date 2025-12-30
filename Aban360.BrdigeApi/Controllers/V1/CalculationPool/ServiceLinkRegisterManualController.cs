using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/service-link")]
    public class ServiceLinkRegisterManualController : BaseController
    {
        private readonly IServiceLinkRegisterManualHandler _serviceLinkRegisterHandler;
        public ServiceLinkRegisterManualController(IServiceLinkRegisterManualHandler serviceLinkRegisterHandler)
        {
            _serviceLinkRegisterHandler = serviceLinkRegisterHandler;
            _serviceLinkRegisterHandler.NotNull(nameof(serviceLinkRegisterHandler));
        }

        [HttpPost, HttpGet]
        [Route("register-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkRegisterManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ManualRegister(ServiceLinkRegisterManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkRegisterHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }
    }
}
