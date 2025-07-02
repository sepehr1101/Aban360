using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/deductions-and-discounts-report")]
    public class DeductionsAndDiscountsReportController : BaseController
    {
        private readonly IDeductionsAndDiscountsReportHandler _deductionsAndDiscountsReportHandler;
        public DeductionsAndDiscountsReportController(IDeductionsAndDiscountsReportHandler deductionsAndDiscountsReportHandler)
        {
            _deductionsAndDiscountsReportHandler = deductionsAndDiscountsReportHandler;
            _deductionsAndDiscountsReportHandler.NotNull(nameof(_deductionsAndDiscountsReportHandler));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDetailDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DeductionsAndDiscountsReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDetailDataOutputDto> deductionsAndDiscountsReport =await _deductionsAndDiscountsReportHandler.Handle(inputDto,cancellationToken);
            return Ok(deductionsAndDiscountsReport);
        }
    }
}
