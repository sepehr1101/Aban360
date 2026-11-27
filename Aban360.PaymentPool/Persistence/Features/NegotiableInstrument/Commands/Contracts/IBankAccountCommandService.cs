using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts
{
    public interface IBankAccountCommandService
    {
        Task Add(BankAccount BankAccount);
        Task Remove(BankAccount BankAccount);
    }
}
