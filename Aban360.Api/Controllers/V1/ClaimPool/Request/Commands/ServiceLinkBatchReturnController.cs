using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/service-link")]
    public class ServiceLinkBatchReturnController : BaseController
    {
        private readonly IServiceLinkRemoveUnconfirmedHandler _serviceLinkRemoveUnconfirmedHandler;
        private readonly IServiceLinkUnconfirmedGetHandler _serviceLinkUnconfirmedGetHandler;
        private readonly IServiceLinkTempReturnHandler _serviceLinkTempReturnHandler;
        private readonly IServiceLinkReturnConfirmeHandler _serviceLinkReturnConfirmeHandler;
        public ServiceLinkBatchReturnController(
            IServiceLinkRemoveUnconfirmedHandler serviceLinkRemoveUnconfirmedHandler,
            IServiceLinkUnconfirmedGetHandler serviceLinkUnconfirmedGetHandler,
            IServiceLinkTempReturnHandler serviceLinkTempReturnHandler,
            IServiceLinkReturnConfirmeHandler serviceLinkReturnConfirmeHandler)
        {
            _serviceLinkRemoveUnconfirmedHandler = serviceLinkRemoveUnconfirmedHandler;
            _serviceLinkRemoveUnconfirmedHandler.NotNull(nameof(serviceLinkRemoveUnconfirmedHandler));

            _serviceLinkUnconfirmedGetHandler = serviceLinkUnconfirmedGetHandler;
            _serviceLinkUnconfirmedGetHandler.NotNull(nameof(serviceLinkUnconfirmedGetHandler));

            _serviceLinkTempReturnHandler = serviceLinkTempReturnHandler;
            _serviceLinkTempReturnHandler.NotNull(nameof(serviceLinkTempReturnHandler));

            _serviceLinkReturnConfirmeHandler = serviceLinkReturnConfirmeHandler;
            _serviceLinkReturnConfirmeHandler.NotNull(nameof(serviceLinkReturnConfirmeHandler));
        }

        [HttpPost]
        [Route("return-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkTempReturnInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Return2([FromBody] ServiceLinkTempReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkTempReturnHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("return-remove-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkRemoveUnconfirmedInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveReturn2([FromBody] ServiceLinkRemoveUnconfirmedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _serviceLinkRemoveUnconfirmedHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("return-unconfirmed-2/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnconfirmedReturn2(string billId, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto> resul = await _serviceLinkUnconfirmedGetHandler.Handle(billId, CurrentUser, cancellationToken);
            return Ok(resul);
        }

        [HttpGet]
        [Route("return-confirmed-2/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmedReturn2(string billId, CancellationToken cancellationToken)
        {
            await _serviceLinkReturnConfirmeHandler.Handle(billId, CurrentUser, cancellationToken);
            return Ok(billId);
        }
    }
}
