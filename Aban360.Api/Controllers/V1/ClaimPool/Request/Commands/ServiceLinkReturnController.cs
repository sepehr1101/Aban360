using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/service-link")]
    public class ServiceLinkReturnController : BaseController
    {
        private readonly IServiceLinkReturnHandler _serviceLinkReturnHandler;
        private readonly IServiceLinkReturnDisconnectHandler _serviceLinkReturnDisconnectHandler;
        public ServiceLinkReturnController(
            IServiceLinkReturnHandler serviceLinkReturnHandler,
            IServiceLinkReturnDisconnectHandler serviceLinkReturnDisconnectHandler)
        {
            _serviceLinkReturnHandler = serviceLinkReturnHandler;
            _serviceLinkReturnHandler.NotNull(nameof(serviceLinkReturnHandler));
            
            _serviceLinkReturnDisconnectHandler = serviceLinkReturnDisconnectHandler;
            _serviceLinkReturnDisconnectHandler.NotNull(nameof(serviceLinkReturnDisconnectHandler));
        }

        [HttpPost]
        [Route("return")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkReturnInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Return([FromBody] ServiceLinkReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkReturnHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
        
        [HttpPost]
        [Route("return-disconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisconnectReturn([FromBody] ServiceLinkReturnDisconnectInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto> result = await _serviceLinkReturnDisconnectHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
