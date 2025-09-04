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
    internal sealed class NonPermanentBranchSummaryByZoneHandler : INonPermanentBranchSummaryByZoneHandler
    {
        private readonly INonPermanentBranchSummaryByZoneQueryService _nonPermanentBranchSummaryByZoneQueryService;
        private readonly IValidator<NonPermanentBranchByUsageAndZoneInputDto> _validator;
        public NonPermanentBranchSummaryByZoneHandler(
            INonPermanentBranchSummaryByZoneQueryService nonPremanentBranchSummaryByZoneQueryService,
            IValidator<NonPermanentBranchByUsageAndZoneInputDto> validator)
        {
            _nonPermanentBranchSummaryByZoneQueryService = nonPremanentBranchSummaryByZoneQueryService;
            _nonPermanentBranchSummaryByZoneQueryService.NotNull(nameof(nonPremanentBranchSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto>> Handle(NonPermanentBranchByUsageAndZoneInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto> nonPremanentBranch = await _nonPermanentBranchSummaryByZoneQueryService.GetInfo(input);
            return nonPremanentBranch;
        }
    }
}
