using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link")]
    public class ServiceLinkPaymentController : BaseController
    {
        private readonly IServiceLinkRegisterManualHandler _serviceLinkRegisterHandler;
        private readonly IServiceLinkDeleteHandler _serviceLinkDeleteHandler;
        private readonly IServiceLinkPaidGetHandler _serviceLinkPaidHandler;
        public ServiceLinkPaymentController(
            IServiceLinkRegisterManualHandler serviceLinkRegisterHandler,
            IServiceLinkDeleteHandler serviceLinkDeleteHandler,
            IServiceLinkPaidGetHandler serviceLinkPaidHandler)
        {
            _serviceLinkRegisterHandler = serviceLinkRegisterHandler;
            _serviceLinkRegisterHandler.NotNull(nameof(serviceLinkRegisterHandler));

            _serviceLinkDeleteHandler = serviceLinkDeleteHandler;
            _serviceLinkDeleteHandler.NotNull(nameof(serviceLinkDeleteHandler));

            _serviceLinkPaidHandler = serviceLinkPaidHandler;
            _serviceLinkPaidHandler.NotNull(nameof(serviceLinkPaidHandler));
        }

        [HttpPost]
        [Route("add-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkRegisterManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPay([FromBody] ServiceLinkRegisterManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkRegisterHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("remove-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkPaymentRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePay([FromBody] ServiceLinkPaymentRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkDeleteHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("paid")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaid([FromBody] ServiceLinkPaidInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto> result = await _serviceLinkPaidHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
