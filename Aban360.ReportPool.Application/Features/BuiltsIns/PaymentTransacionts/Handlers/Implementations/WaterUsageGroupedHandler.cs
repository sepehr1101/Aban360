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
    internal sealed class WaterUsageGroupedHandler : IWaterUsageGroupedHandler
    {
        private readonly IWaterUsageGroupedQueryService _waterUsageGroupedQueryService;
        private readonly IValidator<WaterUsageGroupedInputDto> _validator;
        public WaterUsageGroupedHandler(
            IWaterUsageGroupedQueryService waterUsageGroupedQueryService,
            IValidator<WaterUsageGroupedInputDto> validator)
        {
            _waterUsageGroupedQueryService = waterUsageGroupedQueryService;
            _waterUsageGroupedQueryService.NotNull(nameof(waterUsageGroupedQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterUsageGroupedHeaderOutputDto, WaterUsageGroupedDataOutputDto>> Handle(WaterUsageGroupedInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterUsageGroupedHeaderOutputDto, WaterUsageGroupedDataOutputDto> waterUsageGrouped = await _waterUsageGroupedQueryService.GetInfo(input);
            return waterUsageGrouped;
        }
    }
}
