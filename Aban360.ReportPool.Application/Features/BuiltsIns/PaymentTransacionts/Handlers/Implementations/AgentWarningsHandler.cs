using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class AgentWarningsHandler : IAgentWarningsHandler
    {
        private readonly IAgentWarningsQueryService _agentWarningsQueryService;
        private readonly IValidator<AgentWarningsInputDto> _validator;
        public AgentWarningsHandler(
            IAgentWarningsQueryService agentWarningsQueryService,
            IValidator<AgentWarningsInputDto> validator)
        {
            _agentWarningsQueryService = agentWarningsQueryService;
            _agentWarningsQueryService.NotNull(nameof(agentWarningsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto>> Handle(AgentWarningsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto> agentWarnings = await _agentWarningsQueryService.GetInfo(input);
            return agentWarnings;
        }
    }
}
