using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
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
        private readonly IInstallmentRequestHandler _installmentRequestHandler;
        public InstallmentRequestController(IInstallmentRequestHandler installmentRequestHandler)
        {
            _installmentRequestHandler = installmentRequestHandler;
            _installmentRequestHandler.NotNull(nameof(installmentRequestHandler));
        }

        [HttpPost]
        [Route("installment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InstallmentCalculation([FromBody] InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = await _installmentRequestHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
