using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class PendingPaymentsPrintsHandler : IPendingPaymentsPrintsHandler
    {
        private readonly IPendingPaymentsQueryService _pendingPaymentsQueryService;
        private readonly IValidator<PendingPaymentsInputDto> _validator;
        public PendingPaymentsPrintsHandler(
            IPendingPaymentsQueryService pendingPaymentsQueryService,
            IValidator<PendingPaymentsInputDto> validator)
        {
            _pendingPaymentsQueryService = pendingPaymentsQueryService;
            _pendingPaymentsQueryService.NotNull(nameof(pendingPaymentsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto>> Handle(PendingPaymentsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments = await _pendingPaymentsQueryService.GetInfo(input);

            return GetResult(pendingPayments, input);
        }
        private ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto> GetResult(ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments, PendingPaymentsInputDto input)
        {
            IEnumerable<PendingPaymentPrintsDataOutputDto> data = pendingPayments
                 .ReportData
                 .Where(p => p.EndingDebt >= 0)
                 .Select(p => new PendingPaymentPrintsDataOutputDto()
                 {
                     FirstName = p.FirstName,
                     Surname = p.Surname,
                     CustomerNumber = p.CustomerNumber,
                     ReadingNumber = p.ReadingNumber,
                     UsageTitle = p.UsageConsumptionTitle,
                     MobileNumber = p.MobileNumber,
                     BillId = p.BillId,
                     PayId = TransactionIdGenerator.GeneratePaymentId(p.EndingDebt, p.BillId),
                     DebtAmount = p.EndingDebt,
                     DebtPeriodCount = (int)p.DebtPeriodCount
                 });
            PendingPaymentsPrintstHeaderOutputDto header = new()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromDebtPeriodCount = input.FromDebtPeriodCount,
                ToDebtPeriodCount = input.ToDebtPeriodCount,
                ZoneCount = input.ZoneIds.Count(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                TotalDebtPeriodCount = data.Sum(payment => payment.DebtPeriodCount),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.PendingPayments
            };
            return new ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto>(pendingPayments.Title, header, data);
        }
    }
}
