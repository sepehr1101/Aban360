using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts
{
    public interface IBankAccountCreateHandler
    {
        Task Handle(BankAccountCreateDto createDto, CancellationToken cancellationToken);
    }
}
