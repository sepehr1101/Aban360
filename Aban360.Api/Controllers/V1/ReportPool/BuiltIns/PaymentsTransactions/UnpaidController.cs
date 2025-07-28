using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/unpaid")]
    public class UnpaidController : BaseController
    {
        private readonly IUnpaidHandler _unpaid;
        private readonly IReportGenerator _reportGenerator;
        public UnpaidController(
            IUnpaidHandler unpaid,
            IReportGenerator reportGenerator)
        {
            _unpaid = unpaid;
            _unpaid.NotNull(nameof(_unpaid));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnpaidInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto> unpaid = await _unpaid.Handle(inputDto, cancellationToken);
            return Ok(unpaid);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnpaidInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unpaid.Handle, CurrentUser, ReportLiterals.Unpaid, connectionId);
            return Ok(inputDto);
        }
    }
}
