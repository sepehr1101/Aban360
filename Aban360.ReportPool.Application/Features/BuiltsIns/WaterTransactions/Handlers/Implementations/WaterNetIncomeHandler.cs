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
    internal sealed class WaterNetIncomeHandler : IWaterNetIncomeHandler
    {
        private readonly IWaterNetIncomeQueryService _waterNetIncomeQueryService;
        private readonly IValidator<WaterNetIncomeInputDto> _validator;
        public WaterNetIncomeHandler(
            IWaterNetIncomeQueryService waterNetIncomeQueryService,
            IValidator<WaterNetIncomeInputDto> validator)
        {
            _waterNetIncomeQueryService = waterNetIncomeQueryService;
            _waterNetIncomeQueryService.NotNull(nameof(waterNetIncomeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto>> Handle(WaterNetIncomeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto> WaterNetIncome = await _waterNetIncomeQueryService.Get(input);
            return WaterNetIncome;
        }
    }
}
