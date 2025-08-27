using Aban360.Common.Excel;
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
    internal sealed class WaterMeterReplacementsHandler : IWaterMeterReplacementsHandler
    {
        private readonly IWaterMeterReplacementsQueryService _waterMeterReplacementsQueryService;
        private readonly IValidator<WaterMeterReplacementsInputDto> _validator;
        public WaterMeterReplacementsHandler(
            IWaterMeterReplacementsQueryService waterMeterReplacementsQueryService,
            IValidator<WaterMeterReplacementsInputDto> validator)
        {
            _waterMeterReplacementsQueryService = waterMeterReplacementsQueryService;
            _waterMeterReplacementsQueryService.NotNull(nameof(waterMeterReplacementsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>> Handle(WaterMeterReplacementsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto> waterMeterReplacements = await _waterMeterReplacementsQueryService.GetInfo(input);
            return waterMeterReplacements;
        }
    }
}
