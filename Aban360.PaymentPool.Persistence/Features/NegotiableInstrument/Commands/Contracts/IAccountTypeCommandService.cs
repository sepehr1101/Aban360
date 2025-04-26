using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts
{
    public interface IAccountTypeCommandService
    {
        Task Add(AccountType accountType);
        Task Remove(AccountType accountType);
    }
}
