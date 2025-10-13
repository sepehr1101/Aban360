using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/client-validation")]
    public class ClientValidationController : BaseController
    {
        private readonly IClientValidationHandler _clientValidation;
        private readonly IReportGenerator _reportGenerator;
        public ClientValidationController(
            IClientValidationHandler clientValidation,
            IReportGenerator reportGenerator)
        {
            _clientValidation = clientValidation;
            _clientValidation.NotNull(nameof(_clientValidation));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ClientValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto> clientValidation = await _clientValidation.Handle(inputDto, cancellationToken);
            return Ok(clientValidation);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ClientValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _clientValidation.Handle, CurrentUser, ReportLiterals.ClientValidation, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(ClientValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 340;
            ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto> clientValidation = await _clientValidation.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(clientValidation, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
