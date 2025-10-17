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
    internal sealed class ClientValidationHandler : IClientValidationHandler
    {
        private readonly IClientValidationQueryService _clientValidationQueryService;
        private readonly IValidator<ClientValidationInputDto> _validator;
        public ClientValidationHandler(
            IClientValidationQueryService clientValidationQueryService,
            IValidator<ClientValidationInputDto> validator)
        {
            _clientValidationQueryService = clientValidationQueryService;
            _clientValidationQueryService.NotNull(nameof(clientValidationQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto>> Handle(ClientValidationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ClientValidationHeaderOutputDto, ClientValidationDataOutputDto> clientValidation = await _clientValidationQueryService.GetInfo(input);
            return clientValidation;
        }
    }
}
