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
    internal sealed class UnspecifiedWaterPaymentHandler : IUnspecifiedWaterPaymentHandler
    {
        private readonly IUnspecifiedWaterPaymentQueryService _unspecifiedWaterPaymentQueryService;
        private readonly IValidator<UnspecifiedPaymentInputDto> _validator;
        public UnspecifiedWaterPaymentHandler(
            IUnspecifiedWaterPaymentQueryService unspecifiedWaterPaymentQueryService,
            IValidator<UnspecifiedPaymentInputDto> validator)
        {
            _unspecifiedWaterPaymentQueryService = unspecifiedWaterPaymentQueryService;
            _unspecifiedWaterPaymentQueryService.NotNull(nameof(unspecifiedWaterPaymentQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>> Handle(UnspecifiedPaymentInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto> UnspecefiedServicLink = await _unspecifiedWaterPaymentQueryService.GetInfo(input);
            return UnspecefiedServicLink;
        }
    }
}
