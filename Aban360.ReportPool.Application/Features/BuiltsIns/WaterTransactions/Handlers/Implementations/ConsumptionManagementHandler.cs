using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ConsumptionManagementHandler : IConsumptionManagementHandler
    {
        private readonly IConsumptionManagementQueryService _consumptionManagerQueryService;
        private readonly IValidator<ConsumptionManagementInputDto> _validator;
        public ConsumptionManagementHandler(
            IConsumptionManagementQueryService consumptionManagerQueryService,
            IValidator<ConsumptionManagementInputDto> validator)
        {
            _consumptionManagerQueryService = consumptionManagerQueryService;
            _consumptionManagerQueryService.NotNull(nameof(consumptionManagerQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto>> Handle(ConsumptionManagementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto> result = await _consumptionManagerQueryService.Get(input);

            return result;
        }
    }
}
