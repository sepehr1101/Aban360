using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/bank-grouped")]
    public class BankGroupedController : BaseController
    {
        private readonly IBankGroupedHandler _bankGrouped;
        private readonly IReportGenerator _reportGenerator;
        public BankGroupedController(
            IBankGroupedHandler bankGrouped,
            IReportGenerator reportGenerator)
        {
            _bankGrouped = bankGrouped;
            _bankGrouped.NotNull(nameof(_bankGrouped));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BankGroupedHeaderOutputDto, BankGroupedDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(BankGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BankGroupedHeaderOutputDto, BankGroupedDataOutputDto> BankGrouped = await _bankGrouped.Handle(inputDto, cancellationToken);
            return Ok(BankGrouped);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, BankGroupedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _bankGrouped.Handle, CurrentUser, ReportLiterals.BankGrouped, connectionId);
            return Ok(inputDto);
        }
    }
}
