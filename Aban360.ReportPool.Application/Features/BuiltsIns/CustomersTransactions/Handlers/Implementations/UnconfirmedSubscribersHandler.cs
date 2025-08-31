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
    internal sealed class UnconfirmedSubscribersHandler : IUnconfirmedSubscribersHandler
    {
        private readonly IUnconfirmedSubscribersQueryService _unconfirmedSubscribersQueryService;
        private readonly IValidator<UnconfirmedSubscribersInputDto> _validator;
        public UnconfirmedSubscribersHandler(
            IUnconfirmedSubscribersQueryService unconfirmedSubscribersQueryService,
            IValidator<UnconfirmedSubscribersInputDto> validator)
        {
            _unconfirmedSubscribersQueryService = unconfirmedSubscribersQueryService;
            _unconfirmedSubscribersQueryService.NotNull(nameof(unconfirmedSubscribersQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto>> Handle(UnconfirmedSubscribersInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto> customers = await _unconfirmedSubscribersQueryService.GetInfo(input);
            return customers;
        }
    }
}
