using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/latest-customers-info")]
    public class LatestCustomersInfoController : BaseController
    {
        private readonly ILatestCustomersInfoHandler _latestCustomersInfoHandler;
        private readonly IReportGenerator _reportGenerator;
        public LatestCustomersInfoController(
            ILatestCustomersInfoHandler latestCustomersInfoHandler,
            IReportGenerator reportGenerator)
        {
            _latestCustomersInfoHandler = latestCustomersInfoHandler;
            _latestCustomersInfoHandler.NotNull(nameof(latestCustomersInfoHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, [FromBody] LatestCustomersInfoInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _latestCustomersInfoHandler.Handle, CurrentUser, ReportLiterals.LatestCustomersInfo, connectionId);
            return Ok(inputDto);
        }
    }
}
