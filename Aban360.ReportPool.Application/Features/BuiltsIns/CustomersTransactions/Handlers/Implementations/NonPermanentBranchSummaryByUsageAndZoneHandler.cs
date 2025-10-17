using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class NonPermanentBranchSummaryByUsageAndZoneHandler : INonPermanentBranchSummaryByUsageAndZoneHandler
    {
        private readonly INonPermanentBranchSummaryByUsageAndZoneQueryService _nonPermanentBranchSummaryByUsageAndZoneQueryService;
        private readonly IValidator<NonPermanentBranchByUsageAndZoneInputDto> _validator;
        public NonPermanentBranchSummaryByUsageAndZoneHandler(
            INonPermanentBranchSummaryByUsageAndZoneQueryService nonPremanentBranchSummaryByUsageAndZoneQueryService,
            IValidator<NonPermanentBranchByUsageAndZoneInputDto> validator)
        {
            _nonPermanentBranchSummaryByUsageAndZoneQueryService = nonPremanentBranchSummaryByUsageAndZoneQueryService;
            _nonPermanentBranchSummaryByUsageAndZoneQueryService.NotNull(nameof(nonPremanentBranchSummaryByUsageAndZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchGroupedDataOutputDto>> Handle(NonPermanentBranchByUsageAndZoneInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchGroupedDataOutputDto> nonPremanentBranch = await _nonPermanentBranchSummaryByUsageAndZoneQueryService.GetInfo(input);
            return nonPremanentBranch;
        }
    }
}
