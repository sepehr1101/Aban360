using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.CalculationPool.Application.Features.ServiceLink.Services;
using Aban360.Api.Controllers.V1.CalculationPool.ServiceLink;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link")]
    public class ServiceLinkPaymentController : BaseController
    {
        private readonly IServiceLinkPaymentService _serviceLinkPaymentService;
        public ServiceLinkPaymentController(IServiceLinkPaymentService serviceLinkPaymentService)
        {
            _serviceLinkPaymentService = serviceLinkPaymentService;
            _serviceLinkPaymentService.NotNull(nameof(serviceLinkPaymentService));
        }

        [HttpPost]
        [Route("add-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkRegisterManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPay([FromBody] ServiceLinkRegisterManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkPaymentService.ServiceLinkRegisterManualAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("remove-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkPaymentRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePay([FromBody] ServiceLinkPaymentRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkPaymentService.ServiceLinkDeleteAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("paid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaid([FromBody] ServiceLinkPaidInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto> result = await _serviceLinkPaymentService.ServiceLinkPaidGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
