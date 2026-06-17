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
        private readonly IServiceLinkReturnRemoveHandler _serviceLinkReturnRemoveHandler;
        public ServiceLinkReturnController(
            IServiceLinkReturnHandler serviceLinkReturnHandler,
            IServiceLinkReturnDisconnectHandler serviceLinkReturnDisconnectHandler,
            IServiceLinkReturnRemoveHandler serviceLinkReturnRemoveHandler)
        {
            _serviceLinkReturnHandler = serviceLinkReturnHandler;
            _serviceLinkReturnHandler.NotNull(nameof(serviceLinkReturnHandler));

            _serviceLinkReturnDisconnectHandler = serviceLinkReturnDisconnectHandler;
            _serviceLinkReturnDisconnectHandler.NotNull(nameof(serviceLinkReturnDisconnectHandler));

            _serviceLinkReturnRemoveHandler = serviceLinkReturnRemoveHandler;
            _serviceLinkReturnRemoveHandler.NotNull(nameof(serviceLinkReturnRemoveHandler));
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
        [Route("return-remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkReturnRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveReturn([FromBody] ServiceLinkReturnRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkReturnRemoveHandler.Handle(inputDto, CurrentUser, cancellationToken);
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

        [HttpGet]
        [Route("return-codes")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetReturnCodes(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = _serviceLinkReturnHandler.ReturnCodes();
            return Ok(result);
        }

    }
}
