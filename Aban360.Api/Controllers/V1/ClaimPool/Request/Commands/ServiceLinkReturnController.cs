using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/service-link")]
    public class ServiceLinkReturnController : BaseController
    {
        private readonly IServiceLinkReturnHandler _serviceLinkReturnHandler;
        public ServiceLinkReturnController(IServiceLinkReturnHandler serviceLinkReturnHandler)
        {
            _serviceLinkReturnHandler = serviceLinkReturnHandler;
            _serviceLinkReturnHandler.NotNull(nameof(serviceLinkReturnHandler));
        }

        [HttpPost]
        [Route("return")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkReturnInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Return([FromBody] ServiceLinkReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkReturnHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
