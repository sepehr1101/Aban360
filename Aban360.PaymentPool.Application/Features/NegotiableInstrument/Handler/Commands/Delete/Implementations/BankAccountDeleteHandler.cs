using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Implementations
{
    internal sealed class BankAccountDeleteHandler : IBankAccountDeleteHandler
    {
        private readonly IBankAccountCommandService _bankAccountCommandService;
        private readonly IBankAccountQueryService _bankAccountQueryService;
        public BankAccountDeleteHandler(
            IBankAccountCommandService bankAccountCommandService,
            IBankAccountQueryService bankAccountQueryService)
        {
            _bankAccountCommandService = bankAccountCommandService;
            _bankAccountCommandService.NotNull(nameof(_bankAccountCommandService));

            _bankAccountQueryService = bankAccountQueryService;
            _bankAccountQueryService.NotNull(nameof(_bankAccountQueryService));
        }

        public async Task Handle(BankAccountDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountQueryService.Get(deleteDto.Id);
            await _bankAccountCommandService.Remove(bankAccount);
        }
    }
}
