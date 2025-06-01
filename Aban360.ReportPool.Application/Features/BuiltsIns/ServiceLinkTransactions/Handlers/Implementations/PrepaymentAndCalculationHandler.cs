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
    internal sealed class PrepaymentAndCalculationHandler : IPrepaymentAndCalculationHandler
    {
        private readonly IPrepaymentAndCalculationQueryService _prepaymentAndCalculationQueryService;
        private readonly IValidator<PrepaymentAndCalculationInputDto> _validator;
        public PrepaymentAndCalculationHandler(
            IPrepaymentAndCalculationQueryService prepaymentAndCalculationQueryService,
            IValidator<PrepaymentAndCalculationInputDto> validator)
        {
            _prepaymentAndCalculationQueryService = prepaymentAndCalculationQueryService;
            _prepaymentAndCalculationQueryService.NotNull(nameof(prepaymentAndCalculationQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>> Handle(PrepaymentAndCalculationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto> prepaymentAndCalculation = await _prepaymentAndCalculationQueryService.GetInfo(input);
            return prepaymentAndCalculation;
        }
    }
}
