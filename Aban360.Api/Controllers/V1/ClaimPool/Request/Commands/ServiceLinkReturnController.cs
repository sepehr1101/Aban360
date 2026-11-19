using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.ServiceLink.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/service-link")]
    public class ServiceLinkReturnController : BaseController
    {
        private readonly IServiceLinkReturnService _serviceLinkReturnService;
        public ServiceLinkReturnController(IServiceLinkReturnService serviceLinkReturnService)
        {
            _serviceLinkReturnService = serviceLinkReturnService;
            _serviceLinkReturnService.NotNull(nameof(serviceLinkReturnService));
        }

        [HttpPost]
        [Route("return")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkReturnInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Return([FromBody] ServiceLinkReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkReturnService.ServiceLinkReturnAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("return-remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkReturnRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveReturn([FromBody] ServiceLinkReturnRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkReturnService.ServiceLinkReturnRemoveAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("return-disconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisconnectReturn([FromBody] ServiceLinkReturnDisconnectInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto> result = await _serviceLinkReturnService.ServiceLinkReturnDisconnectAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("return-codes")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetReturnCodes(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = _serviceLinkReturnService.ServiceLinkReturnReturnCodesAsync();
            return Ok(result);
        }
    }
}
