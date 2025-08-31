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
    [Route("v1/prepayment-and-calculation")]
    public class PrepaymentAndCalculationController:BaseController
    {
        private readonly IPrepaymentAndCalculationHandler _prepaymentAndCalculationHandler;
        private readonly IReportGenerator _reportGenerator;
        public PrepaymentAndCalculationController(
            IPrepaymentAndCalculationHandler prepaymentAndCalculationHandler,
            IReportGenerator reportGenerator)
        {
            _prepaymentAndCalculationHandler = prepaymentAndCalculationHandler;
            _prepaymentAndCalculationHandler.NotNull(nameof(_prepaymentAndCalculationHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>>),StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRaw(PrepaymentAndCalculationInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto> prepaymentAndCalculation =await _prepaymentAndCalculationHandler.Handle(inputDto,cancellationToken);
            return Ok(prepaymentAndCalculation);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PrepaymentAndCalculationInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _prepaymentAndCalculationHandler.Handle, CurrentUser, ReportLiterals.PrepaymentAndCalculation, connectionId);
            return Ok(inputDto);
        }
    }
}
