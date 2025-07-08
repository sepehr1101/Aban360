using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ExcessPatternHandler : IExcessPatternHandler
    {
        private readonly IExcessPatternQueryService _excessPatternQueryService;
        private readonly IValidator<ExcessPatternInputDto> _validator;
        public ExcessPatternHandler(
            IExcessPatternQueryService excessPatternQueryService,
            IValidator<ExcessPatternInputDto> validator)
        {
            _excessPatternQueryService = excessPatternQueryService;
            _excessPatternQueryService.NotNull(nameof(excessPatternQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto>> Handle(ExcessPatternInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto> excessPattern = await _excessPatternQueryService.GetInfo(input);
            return excessPattern;
        }
    }
}
