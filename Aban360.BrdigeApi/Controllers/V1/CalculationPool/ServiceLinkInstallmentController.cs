using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/service-link")]
    public class ServiceLinkInstallmentController : BaseController
    {
        private readonly IServiceLinkInstallmentHandler _serviceLinkInstallmentHandler;
        public ServiceLinkInstallmentController(IServiceLinkInstallmentHandler serviceLinkInstallmentHandler)
        {
            _serviceLinkInstallmentHandler = serviceLinkInstallmentHandler;
            _serviceLinkInstallmentHandler.NotNull(nameof(serviceLinkInstallmentHandler));
        }

        [HttpPost]
        [Route("installment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Installment([FromBody] InstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto> result = await _serviceLinkInstallmentHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
