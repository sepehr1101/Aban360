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
    [Route("v1/non-premanent-branch")]
    public class NonPermanentBranchController : BaseController
    {
        private readonly INonPermanentBranchHandler _nonPermanentBranch;
        private readonly IReportGenerator _reportGenerator;
        public NonPermanentBranchController(INonPermanentBranchHandler nonPremanentBranch,
            IReportGenerator reportGenerator)
        {
            _nonPermanentBranch = nonPremanentBranch;
            _nonPermanentBranch.NotNull(nameof(_nonPermanentBranch));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto> nonPermanentBranch = await _nonPermanentBranch.Handle(inputDto, cancellationToken);
            return Ok(nonPermanentBranch);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, NonPermanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _nonPermanentBranch.Handle, CurrentUser, ReportLiterals.NonPermanentBranchDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(NonPermanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 10;
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto> nonPermanentBranch = await _nonPermanentBranch.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(nonPermanentBranch, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
