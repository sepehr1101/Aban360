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
    internal sealed class MeterReplacementLifeSummaryHandler : IMeterReplacementLifeSummaryHandler
    {
        private readonly IMeterReplacementLifeSummaryQueryService _meterReplacementLifeService;
        private readonly IValidator<MeterReplacementLifeInputDto> _validator;
        public MeterReplacementLifeSummaryHandler(
            IMeterReplacementLifeSummaryQueryService meterReplacementLifeService,
            IValidator<MeterReplacementLifeInputDto> validator)
        {
            _meterReplacementLifeService = meterReplacementLifeService;
            _meterReplacementLifeService.NotNull(nameof(meterReplacementLifeService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto>> Handle(MeterReplacementLifeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MeterReplacementLifeHeaderOutputDto, MeterReplacementLifeDataOutputDto> result = await _meterReplacementLifeService.Get(input);
            //header
            return result;
        }
    }
}
