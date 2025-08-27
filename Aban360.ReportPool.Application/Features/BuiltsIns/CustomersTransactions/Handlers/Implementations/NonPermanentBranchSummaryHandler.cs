using Aban360.Common.Excel;
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
    internal sealed class NonPermanentBranchSummaryHandler : INonPermanentBranchSummaryHandler
    {
        private readonly INonPermanentBranchSummaryQueryService _nonPermanentBranchSummaryQueryService;
        private readonly IValidator<NonPermanentBranchInputDto> _validator;
        public NonPermanentBranchSummaryHandler(
            INonPermanentBranchSummaryQueryService nonPremanentBranchSummaryQueryService,
            IValidator<NonPermanentBranchInputDto> validator)
        {
            _nonPermanentBranchSummaryQueryService = nonPremanentBranchSummaryQueryService;
            _nonPermanentBranchSummaryQueryService.NotNull(nameof(nonPremanentBranchSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto>> Handle(NonPermanentBranchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto> nonPremanentBranch = await _nonPermanentBranchSummaryQueryService.GetInfo(input);
            return nonPremanentBranch;
        }
    }
}
