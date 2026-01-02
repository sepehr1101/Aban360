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
    internal sealed class MeterDuplicateChangeDetailHandler : IMeterDuplicateChangeDetailHandler
    {
        private readonly IMeterDuplicateChangeDetailQueryService _meterDuplicateService;
        private readonly IValidator<MeterDuplicateChangeInputDto> _validator;
        public MeterDuplicateChangeDetailHandler(
            IMeterDuplicateChangeDetailQueryService meterDuplicateService,
            IValidator<MeterDuplicateChangeInputDto> validator)
        {
            _meterDuplicateService = meterDuplicateService;
            _meterDuplicateService.NotNull(nameof(meterDuplicateService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto>> Handle(MeterDuplicateChangeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MeterDuplicateChangeHeaderOutputDto, MeterDuplicateChangeDetailDataOutputDto> result = await _meterDuplicateService.GetInfo(input);
            return result;
        }
    }
}
