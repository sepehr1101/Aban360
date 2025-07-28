using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/sewage-water-distance-request-installation-detail")]
    public class SewageWaterDistanceofRequestAndInstallationDetailController : BaseController
    {
        private readonly ISewageWaterDistanceofRequestAndInstallationDetailHandler _sewageWaterDistanceofRequestAndInstallationDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public SewageWaterDistanceofRequestAndInstallationDetailController(
            ISewageWaterDistanceofRequestAndInstallationDetailHandler sewageWaterDistanceofRequestAndInstallationDetailHandler,
            IReportGenerator reportGenerator)
        {
            _sewageWaterDistanceofRequestAndInstallationDetailHandler = sewageWaterDistanceofRequestAndInstallationDetailHandler;
            _sewageWaterDistanceofRequestAndInstallationDetailHandler.NotNull(nameof(sewageWaterDistanceofRequestAndInstallationDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(SewageWaterDistanceofRequestAndInstallationInputDto input,CancellationToken cancellationToken)
        {
            var result=await _sewageWaterDistanceofRequestAndInstallationDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SewageWaterDistanceofRequestAndInstallationInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = inputDto.IsWater ? ReportLiterals.WaterDistanceRequestInstallationDetail : ReportLiterals.SewageDistanceRequesteInstallationDetail;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _sewageWaterDistanceofRequestAndInstallationDetailHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
