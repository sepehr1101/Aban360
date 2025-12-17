using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class PaymentInquiryHandler : IPaymentInquiryHandler
    {
        private readonly IPaymentInquiryQueryService _paymentInquiryQueryService;
        private readonly IValidator<PaymentInquiryInputDto> _validator;
        public PaymentInquiryHandler(
            IPaymentInquiryQueryService paymentInquiryQueryService,
            IValidator<PaymentInquiryInputDto> validator)
        {
            _paymentInquiryQueryService = paymentInquiryQueryService;
            _paymentInquiryQueryService.NotNull(nameof(paymentInquiryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto>> Handle(PaymentInquiryInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto> PaymentInquiry = await _paymentInquiryQueryService.GetInfo(input);
            return PaymentInquiry;
        }
    }
}
