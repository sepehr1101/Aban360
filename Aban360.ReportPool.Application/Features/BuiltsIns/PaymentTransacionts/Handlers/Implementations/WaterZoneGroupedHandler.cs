using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class WaterZoneGroupedHandler : IWaterZoneGroupedHandler
    {
        private readonly IWaterZoneGroupedQueryService _waterZoneGroupedQueryService;
        private readonly IValidator<SewageWaterItemGroupedInputDto> _validator;
        public WaterZoneGroupedHandler(
            IWaterZoneGroupedQueryService waterZoneGroupedQueryService,
            IValidator<SewageWaterItemGroupedInputDto> validator)
        {
            _waterZoneGroupedQueryService = waterZoneGroupedQueryService;
            _waterZoneGroupedQueryService.NotNull(nameof(waterZoneGroupedQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterItemGroupedHeaderOutputDto, SewageWaterItemGroupedDataOutputDto>> Handle(SewageWaterItemGroupedInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<SewageWaterItemGroupedHeaderOutputDto, SewageWaterItemGroupedDataOutputDto> waterZoneGrouped = await _waterZoneGroupedQueryService.GetInfo(input);
            return waterZoneGrouped;
        }
    }
}