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
    internal sealed class ClientGuildSummaryHandler : IClientGuildSummaryHandler
    {
        private readonly IClientGuildQueryService _clientGuildSummaryQueryService;
        private readonly IValidator<ClientGuildInputDto> _validator;
        public ClientGuildSummaryHandler(
            IClientGuildQueryService clientGuildSummaryQueryService,
            IValidator<ClientGuildInputDto> validator)
        {
            _clientGuildSummaryQueryService = clientGuildSummaryQueryService;
            _clientGuildSummaryQueryService.NotNull(nameof(clientGuildSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto>> Handle(ClientGuildInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto> result = await _clientGuildSummaryQueryService.GetSummary(input);
            return result;
        }
    }
}
