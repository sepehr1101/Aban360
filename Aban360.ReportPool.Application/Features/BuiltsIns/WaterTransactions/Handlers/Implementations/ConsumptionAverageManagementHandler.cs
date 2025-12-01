using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ConsumptionAverageManagementHandler : IConsumptionAverageManagementHandler
    {
        private readonly IConsumptionAverageManagementQueryService _consumptionAverageManagerQueryService;
        private readonly IValidator<ConsumptionAverageManagementInputDto> _validator;
        public ConsumptionAverageManagementHandler(
            IConsumptionAverageManagementQueryService consumptionAverageManagerQueryService,
            IValidator<ConsumptionAverageManagementInputDto> validator)
        {
            _consumptionAverageManagerQueryService = consumptionAverageManagerQueryService;
            _consumptionAverageManagerQueryService.NotNull(nameof(consumptionAverageManagerQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementDataOutputDto>> Handle(ConsumptionAverageManagementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementDataOutputDto> result = await _consumptionAverageManagerQueryService.Get(input);

            return result;
        }
    }
}
