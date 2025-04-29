using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Implementations
{
    internal sealed class CreditDeleteHandler : ICreditDeleteHandler
    {
        private readonly ICreditCommandService _creditCommandService;
        private readonly ICreditQueryService _creditQueryService;
        public CreditDeleteHandler(
            ICreditCommandService creditCommandService,
            ICreditQueryService creditQueryService)
        {
            _creditCommandService = creditCommandService;
            _creditCommandService.NotNull(nameof(_creditCommandService));

            _creditQueryService = creditQueryService;
            _creditQueryService.NotNull(nameof(_creditQueryService));
        }

        public async Task Handle(CreditDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var credit = await _creditQueryService.Get(deleteDto.Id);
            await _creditCommandService.Remove(credit);
        }
    }
}
