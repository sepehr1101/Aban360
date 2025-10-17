using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class UsageSummaryHandler : IUsageSummaryHandler
    {
        private readonly IUsageSummaryQueryService _usageSummaryQueryService;
        private readonly IValidator<UsageSummaryInputDto> _validator;
        public UsageSummaryHandler(
            IUsageSummaryQueryService usageSummaryQueryService,
            IValidator<UsageSummaryInputDto> validator)
        {
            _usageSummaryQueryService = usageSummaryQueryService;
            _usageSummaryQueryService.NotNull(nameof(usageSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto>> Handle(UsageSummaryInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<UsageSummaryHeaderOutputDto, UsageSummaryDataOutputDto> usageSummary = await _usageSummaryQueryService.GetInfo(input);
            return usageSummary;
        }
    }
}
