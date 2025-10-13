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
    [Route("v1/without-sewage-request-detail")]
    public class WithoutSewageRequestDetailController : BaseController
    {
        private readonly IWithoutSewageRequestDetailHandler _withoutSewageRequestDetailHandler;
        private readonly IReportGenerator _reportGenerator;
        public WithoutSewageRequestDetailController(
            IWithoutSewageRequestDetailHandler withoutSewageRequestDetailHandler,
            IReportGenerator reportGenerator)
        {
            _withoutSewageRequestDetailHandler = withoutSewageRequestDetailHandler;
            _withoutSewageRequestDetailHandler.NotNull(nameof(withoutSewageRequestDetailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(WithoutSewageRequestInputDto input,CancellationToken cancellationToken)
        {
            ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto> result =await _withoutSewageRequestDetailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, WithoutSewageRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _withoutSewageRequestDetailHandler.Handle, CurrentUser, ReportLiterals.WithoutSewageRequestDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(WithoutSewageRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 620;
            ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestDetailDataOutputDto> result = await _withoutSewageRequestDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
