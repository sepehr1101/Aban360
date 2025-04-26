using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts
{
    public interface IBankAccountDeleteHandler
    {
        Task Handle(BankAccountDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
