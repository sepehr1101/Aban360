using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-installation-detail")]
    public class SewageWaterInstallationDetailController : BaseController
    {
        private readonly ISewageWaterInstallationDetailHandler _sewageWaterInstallationDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterInstallationDetailController(
            ISewageWaterInstallationDetailHandler sewageWaterInstallationDetailHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterInstallationDetailHandler = sewageWaterInstallationDetailHandler;
            _sewageWaterInstallationDetailHandler.NotNull(nameof(sewageWaterInstallationDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterInstallationInputDto input,CancellationToken cancellationToken)
        {
            var result=await _sewageWaterInstallationDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterInstallationDetail : ReportLiterals.SewageInstallationDetail;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterInstallationDetailHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SewageWaterInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 240;
            ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto> calculationDetails = await _sewageWaterInstallationDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(calculationDetails, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
