using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MeterChangeVerificationDetailHandler : IMeterChangeVerificationDetailHandler
    {
        private readonly IMeterChangeVerificationDetailQueryService _meterChangeVerificationService;
        private readonly IValidator<MeterChangeVerificationInputDto> _validator;
        public MeterChangeVerificationDetailHandler(
            IMeterChangeVerificationDetailQueryService MeterChangeVerificationService,
            IValidator<MeterChangeVerificationInputDto> validator)
        {
            _meterChangeVerificationService = MeterChangeVerificationService;
            _meterChangeVerificationService.NotNull(nameof(MeterChangeVerificationService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto>> Handle(MeterChangeVerificationInputDto input, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto> result = await _meterChangeVerificationService.GetInfo(input);

            return result;
        }
        public async Task InputValidate(MeterChangeVerificationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
