using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class UnspecifiedServiceLinkPaymentHandler : IUnspecifiedServiceLinkPaymentHandler
    {
        private readonly IUnspecifiedServiceLinkPaymentQueryService _unspecifiedServiceLinkPaymentQueryService;
        private readonly IValidator<UnspecifiedPaymentInputDto> _validator;
        public UnspecifiedServiceLinkPaymentHandler(
            IUnspecifiedServiceLinkPaymentQueryService unspecifiedServiceLinkPaymentQueryService,
            IValidator<UnspecifiedPaymentInputDto> validator)
        {
            _unspecifiedServiceLinkPaymentQueryService = unspecifiedServiceLinkPaymentQueryService;
            _unspecifiedServiceLinkPaymentQueryService.NotNull(nameof(unspecifiedServiceLinkPaymentQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>> Handle(UnspecifiedPaymentInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto> UnspecefiedServicLink = await _unspecifiedServiceLinkPaymentQueryService.GetInfo(input);
            return UnspecefiedServicLink;
        }
    }
}
