using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-installation-department-summary-by-zone")]
    public class SewageWaterInstallationDepartmentSummaryByZoneController : BaseController
    {
        private readonly ISewageWaterInstallationDepartmentSummaryByZoneHandler _sewageWaterInstallationDepartmentSummaryByZoneIdHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterInstallationDepartmentSummaryByZoneController(
            ISewageWaterInstallationDepartmentSummaryByZoneHandler sewageWaterInstallationDepartmentSummaryByZoneIdHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterInstallationDepartmentSummaryByZoneIdHandler = sewageWaterInstallationDepartmentSummaryByZoneIdHandler;
            _sewageWaterInstallationDepartmentSummaryByZoneIdHandler.NotNull(nameof(sewageWaterInstallationDepartmentSummaryByZoneIdHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto> result = await _sewageWaterInstallationDepartmentSummaryByZoneIdHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = (inputDto.IsWater ? ReportLiterals.WaterInstallationDepartmentSummary : ReportLiterals.SewageInstallationDepartmentSummary) + ReportLiterals.ByZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterInstallationDepartmentSummaryByZoneIdHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 242;
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto> calculationDetails = await _sewageWaterInstallationDepartmentSummaryByZoneIdHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
