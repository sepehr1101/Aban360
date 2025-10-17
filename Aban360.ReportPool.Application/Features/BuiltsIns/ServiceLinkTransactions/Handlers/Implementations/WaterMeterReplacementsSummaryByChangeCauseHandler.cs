using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class WaterMeterReplacementsSummaryByChangeCauseHandler : IWaterMeterReplacementsSummaryByChangeCauseHandler
    {
        private readonly IWaterMeterReplacementsSummaryByChangeCauseQueryService _waterMeterReplacementsSummaryByChangeCauseQueryService;
        private readonly IValidator<WaterMeterReplacementsInputDto> _validator;
        public WaterMeterReplacementsSummaryByChangeCauseHandler(
            IWaterMeterReplacementsSummaryByChangeCauseQueryService waterMeterReplacementsSummaryByChangeCauseQueryService,
            IValidator<WaterMeterReplacementsInputDto> validator)
        {
            _waterMeterReplacementsSummaryByChangeCauseQueryService = waterMeterReplacementsSummaryByChangeCauseQueryService;
            _waterMeterReplacementsSummaryByChangeCauseQueryService.NotNull(nameof(waterMeterReplacementsSummaryByChangeCauseQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto>> Handle(WaterMeterReplacementsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto> waterMeterReplacementsSummaryByChangeCause = await _waterMeterReplacementsSummaryByChangeCauseQueryService.Get(input);
            return waterMeterReplacementsSummaryByChangeCause;
        }
    }
}
