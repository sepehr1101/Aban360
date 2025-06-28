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
    internal sealed class NonPermanentBranchHandler : INonPermanentBranchHandler
    {
        private readonly INonPermanentBranchQueryService _nonPermanentBranchQueryService;
        private readonly IValidator<NonPermanentBranchInputDto> _validator;
        public NonPermanentBranchHandler(
            INonPermanentBranchQueryService nonPremanentBranchQueryService,
            IValidator<NonPermanentBranchInputDto> validator)
        {
            _nonPermanentBranchQueryService = nonPremanentBranchQueryService;
            _nonPermanentBranchQueryService.NotNull(nameof(nonPremanentBranchQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>> Handle(NonPermanentBranchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto> nonPremanentBranch = await _nonPermanentBranchQueryService.GetInfo(input);
            return nonPremanentBranch;
        }
    }
}
