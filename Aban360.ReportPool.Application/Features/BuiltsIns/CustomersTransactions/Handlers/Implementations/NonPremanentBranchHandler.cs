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
    internal sealed class NonPremanentBranchHandler : INonPremanentBranchHandler
    {
        private readonly INonPremanentBranchQueryService _nonPremanentBranchQueryService;
        private readonly IValidator<NonPremanentBranchInputDto> _validator;
        public NonPremanentBranchHandler(
            INonPremanentBranchQueryService nonPremanentBranchQueryService,
            IValidator<NonPremanentBranchInputDto> validator)
        {
            _nonPremanentBranchQueryService = nonPremanentBranchQueryService;
            _nonPremanentBranchQueryService.NotNull(nameof(nonPremanentBranchQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<NonPremanentBranchHeaderOutputDto, NonPremanentBranchDataOutputDto>> Handle(NonPremanentBranchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<NonPremanentBranchHeaderOutputDto, NonPremanentBranchDataOutputDto> nonPremanentBranch = await _nonPremanentBranchQueryService.GetInfo(input);
            return nonPremanentBranch;
        }
    }
}
