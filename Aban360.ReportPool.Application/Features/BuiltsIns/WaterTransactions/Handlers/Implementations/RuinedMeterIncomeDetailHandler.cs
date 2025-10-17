using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class RuinedMeterIncomeDetailHandler : IRuinedMeterIncomeDetailHandler
    {
        private readonly IRuinedMeterIncomeDetailQueryService _ruinedMeterIncomeQueryService;
        private readonly IValidator<RuinedMeterIncomeInputDto> _validator;
        public RuinedMeterIncomeDetailHandler(
            IRuinedMeterIncomeDetailQueryService ruinedMeterIncomeQueryService,
            IValidator<RuinedMeterIncomeInputDto> validator)
        {
            _ruinedMeterIncomeQueryService = ruinedMeterIncomeQueryService;
            _ruinedMeterIncomeQueryService.NotNull(nameof(ruinedMeterIncomeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto>> Handle(RuinedMeterIncomeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto> ruinedMeterIncome = await _ruinedMeterIncomeQueryService.GetInfo(input);
            return ruinedMeterIncome;
        }
    }
}
