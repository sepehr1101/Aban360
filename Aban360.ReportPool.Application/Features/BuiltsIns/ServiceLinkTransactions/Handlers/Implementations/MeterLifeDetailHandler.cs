using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class MeterLifeDetailHandler : IMeterLifeDetailHandler
    {
        private readonly IMeterLifeService _meterLifeService;
        private readonly IValidator<MeterLifeInputDto> _validator;
        public MeterLifeDetailHandler(
            IMeterLifeService meterLifeService,
            IValidator<MeterLifeInputDto> validator)
        {
            _meterLifeService = meterLifeService;
            _meterLifeService.NotNull(nameof(meterLifeService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto>> Handle(MeterLifeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto> result = await _meterLifeService.Get(input);
            result.ReportHeader.AverageLifeText = CalculationDistanceDate.ConvertDayToDate(result.ReportHeader.AverageLifeInDay);
            result.ReportHeader.MaxLifeText = CalculationDistanceDate.ConvertDayToDate(result.ReportHeader.MaxLifeInDay);
            result.ReportHeader.MinLifeText = CalculationDistanceDate.ConvertDayToDate(result.ReportHeader.MinLifeInDay);

            return result;
        }
    }
}
