using Aban360.Common.Excel;
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
    internal sealed class UnpaidHandler : IUnpaidHandler
    {
        private readonly IUnpaidQueryService _unpaidQueryService;
        private readonly IValidator<UnpaidInputDto> _validator;
        public UnpaidHandler(
            IUnpaidQueryService unpaidQueryService,
            IValidator<UnpaidInputDto> validator)
        {
            _unpaidQueryService = unpaidQueryService;
            _unpaidQueryService.NotNull(nameof(unpaidQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>> Handle(UnpaidInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto> unpaid = await _unpaidQueryService.GetInfo(input);
            return unpaid;
        }
    }
}
