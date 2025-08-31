using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class DeductionsAndDiscountsSummaryReportHandler : IDeductionsAndDiscountsSummaryReportHandler
    {
        private readonly IDeductionsAndDiscountsReportSummaryQueryService _deductionsAndDiscountsReportQueryService;
        private readonly IValidator<DeductionsAndDiscountsReportInputDto> _validator;
        public DeductionsAndDiscountsSummaryReportHandler(
            IDeductionsAndDiscountsReportSummaryQueryService deductionsAndDiscountsReportQueryService,
            IValidator<DeductionsAndDiscountsReportInputDto> validator)
        {
            _deductionsAndDiscountsReportQueryService = deductionsAndDiscountsReportQueryService;
            _deductionsAndDiscountsReportQueryService.NotNull(nameof(deductionsAndDiscountsReportQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportSummaryDataOutputDto>> Handle(DeductionsAndDiscountsReportInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<DeductionsAndDiscountsReportHeaderOutputDto, DeductionsAndDiscountsReportSummaryDataOutputDto> deductionsAndDiscountsReport = await _deductionsAndDiscountsReportQueryService.GetInfo(input);
            return deductionsAndDiscountsReport;
        }
    }
}
