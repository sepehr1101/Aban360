using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class InstallmentRequestController : BaseController
    {
        private readonly ISetInstallmentRequestHandler _installmentRequestHandler;
        private readonly IInstallmentDisplayHandler _installmentDisplayHandler;
        public InstallmentRequestController(
            ISetInstallmentRequestHandler installmentRequestHandler,
            IInstallmentDisplayHandler installmentDisplayHandler)
        {
            _installmentRequestHandler = installmentRequestHandler;
            _installmentRequestHandler.NotNull(nameof(installmentRequestHandler));

            _installmentDisplayHandler = installmentDisplayHandler;
            _installmentDisplayHandler.NotNull(nameof(installmentDisplayHandler));
        }

        [HttpPost]
        [Route("installment-set")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InstallmentCalculation([FromBody] InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = await _installmentRequestHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("installment-display")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InstallmentDisplay([FromBody] SearchByIdInput inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = await _installmentDisplayHandler.Handle(inputDto.Id, cancellationToken);
            return Ok(result);
        }
    }
}
