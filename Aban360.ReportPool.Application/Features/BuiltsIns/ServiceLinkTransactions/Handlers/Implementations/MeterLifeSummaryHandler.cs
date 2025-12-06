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
    internal sealed class MeterLifeSummaryHandler : IMeterLifeSummaryHandler
    {
        private readonly IMeterLifeService _meterLifeService;
        private readonly IValidator<MeterLifeInputDto> _validator;
        public MeterLifeSummaryHandler(
            IMeterLifeService meterLifeService,
            IValidator<MeterLifeInputDto> validator)
        {
            _meterLifeService = meterLifeService;
            _meterLifeService.NotNull(nameof(meterLifeService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterLifeSummaryHeaderOutputDto, MeterLifeSummaryDataOutputDto>> Handle(MeterLifeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            input.FromLifeInDay = input.FromLifeInDay * 365;
            input.ToLifeInDay = input.ToLifeInDay * 365;
            ReportOutput<MeterLifeSummaryHeaderOutputDto, MeterLifeSummaryDataOutputDto> result = await _meterLifeService.GetSummary(input);

            return result;
        }
    }
}
