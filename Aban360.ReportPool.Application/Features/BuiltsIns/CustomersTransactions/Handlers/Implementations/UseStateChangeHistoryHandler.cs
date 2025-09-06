using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class UseStateChangeHistoryHandler : IUseStateChangeHistoryHandler
    {
        private readonly IUseStateChangeHistoryQueryService _useStateChangeHistoryQueryService;
        private readonly IValidator<UseStateChangeHistoryInputDto> _validator;
        public UseStateChangeHistoryHandler(
            IUseStateChangeHistoryQueryService useStateChangeHistoryQueryService,
            IValidator<UseStateChangeHistoryInputDto> validator)
        {
            _useStateChangeHistoryQueryService = useStateChangeHistoryQueryService;
            _useStateChangeHistoryQueryService.NotNull(nameof(useStateChangeHistoryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UseStateChangeHistoryHeaderOutputDto, UseStateChangeHistoryDataOutputDto>> Handle(UseStateChangeHistoryInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var messeState = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(messeState);
            }

            ReportOutput<UseStateChangeHistoryHeaderOutputDto, UseStateChangeHistoryDataOutputDto> useStateChangeHistory = await _useStateChangeHistoryQueryService.GetInfo(input);
            return useStateChangeHistory;
        }
    }
}
