using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/basic-info-change-history")]
    public class BasicInfoChangeHistoryController : BaseController
    {
        private readonly IBasicInfoChangeHistoryHandler _basicInfoChangeHistory;
        private readonly IReportGenerator _reportGenerator;
        public BasicInfoChangeHistoryController(
            IBasicInfoChangeHistoryHandler BasicInfoChangeHistory,
            IReportGenerator reportGenerator)
        {
            _basicInfoChangeHistory = BasicInfoChangeHistory;
            _basicInfoChangeHistory.NotNull(nameof(_basicInfoChangeHistory));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BasicInfoChangeHistoryHeaderOutputDto, BasicInfoChangeHistoryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(BasicInfoChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BasicInfoChangeHistoryHeaderOutputDto, BasicInfoChangeHistoryDataOutputDto> BasicInfoChangeHistory = await _basicInfoChangeHistory.Handle(inputDto, cancellationToken);
            return Ok(BasicInfoChangeHistory);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, BasicInfoChangeHistoryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _basicInfoChangeHistory.Handle, CurrentUser, ReportLiterals.BasicInfoChangeHistory, connectionId);
            return Ok(inputDto);
        }
    }
}
