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
    internal sealed class ClientGuildDetailHandler : IClientGuildDetailHandler
    {
        private readonly IClientGuildQueryService _clientGuildDetailQueryService;
        private readonly IValidator<ClientGuildInputDto> _validator;
        public ClientGuildDetailHandler(
            IClientGuildQueryService clientGuildDetailQueryService,
            IValidator<ClientGuildInputDto> validator)
        {
            _clientGuildDetailQueryService = clientGuildDetailQueryService;
            _clientGuildDetailQueryService.NotNull(nameof(clientGuildDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto>> Handle(ClientGuildInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto> result = await _clientGuildDetailQueryService.GetDetail(input);
            return result;
        }
    }
}
