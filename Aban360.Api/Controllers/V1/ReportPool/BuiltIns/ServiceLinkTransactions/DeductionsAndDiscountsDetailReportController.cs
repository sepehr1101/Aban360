using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/deductions-and-discounts-detail-report")]
    public class DeductionsAndDiscountsDetailReportController : BaseController
    {
        private readonly IDeductionsAndDiscountsDetailReportHandler _deductionsAndDiscountsDetailReportHandler;
        private readonly IReportGenerator _reportGenerator;
        public DeductionsAndDiscountsDetailReportController(
            IDeductionsAndDiscountsDetailReportHandler deductionsAndDiscountsDetailReportHandler,
            IReportGenerator reportGenerator)
        {
            _deductionsAndDiscountsDetailReportHandler = deductionsAndDiscountsDetailReportHandler;
            _deductionsAndDiscountsDetailReportHandler.NotNull(nameof(_deductionsAndDiscountsDetailReportHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDetailDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(DeductionsAndDiscountsReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportDetailDataOutputDto> deductionsAndDiscountsDetailReport =await _deductionsAndDiscountsDetailReportHandler.Handle(inputDto,cancellationToken);
            return Ok(deductionsAndDiscountsDetailReport);
        }

        [HttpPost, HttpGet]
        [Route("eDeductionsAndDiscountcel/{connectionId}")]
        public async Task<IActionResult> GetEDeductionsAndDiscountcel(string connectionId, DeductionsAndDiscountsReportInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _deductionsAndDiscountsDetailReportHandler.Handle, CurrentUser, ReportLiterals.DeductionsAndDiscountsReportDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
