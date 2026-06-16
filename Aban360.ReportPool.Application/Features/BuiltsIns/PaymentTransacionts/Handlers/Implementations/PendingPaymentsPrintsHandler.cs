using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
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
        private readonly ILocationInfoGetHandler _locationInfoService;
        private readonly IValidator<PendingPaymentsInputDto> _validator;
        public PendingPaymentsPrintsHandler(
            IPendingPaymentsQueryService pendingPaymentsQueryService,
            ILocationInfoGetHandler locationInfoService,
            IValidator<PendingPaymentsInputDto> validator)
        {
            _pendingPaymentsQueryService = pendingPaymentsQueryService;
            _pendingPaymentsQueryService.NotNull(nameof(pendingPaymentsQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

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
            if (!pendingPayments.ReportData.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundData);
            }

            return GetResult(pendingPayments, input);
        }
        private ReportOutput<PendingPaymentsPrintstHeaderOutputDto, PendingPaymentPrintsDataOutputDto> GetResult(ReportOutput<PendingPaymentsHeaderOutputDto, PendingPaymentsDataOutputDto> pendingPayments, PendingPaymentsInputDto input)
        {
            IEnumerable<PendingPaymentPrintsDataOutputDto> data = pendingPayments
                 .ReportData
                 .Where(p => p.EndingDebt >= 0)
                 .Select(p => new PendingPaymentPrintsDataOutputDto()
                 {
                     RegionTitle = p.RegionTitle,
                     ZoneTitle = p.ZoneTitle,
                     ZoneAddress=p.ZoneAddress,
                     FullName = $"{p.FirstName} {p.Surname}",
                     FatherName = p.FatherName,
                     NationalCode = p.NationalCode,
                     UsageTitle = p.UsageSellTitle,
                     CauseTitle = "بدهی مشترک",
                     CustomerNumber = p.CustomerNumber,
                     CustomerNumber2 = p.CustomerNumber,
                     ReadingNumber = p.ReadingNumber,
                     DebtAmount = p.EndingDebt,
                     FirstName = p.FirstName,
                     Surname = p.Surname,
                     PostalCode = p.PostalCode,
                     Address = p.Address,
                     UsageId = p.UsageSellId,
                     PhoneNumber = p.PhoneNumber,
                     MobileNumber = p.MobileNumber,
                     DebtPeriodCount = (int)p.DebtPeriodCount,
                     PreviousReadingDateJalali = string.Empty,//todo
                     PreviousBillAmount = 0,//todo
                     DueDateJalali = string.Empty,//todo
                     BillId = p.BillId,
                     PayId = TransactionIdGenerator.GeneratePaymentId(p.EndingDebt, p.BillId, "100"),
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
