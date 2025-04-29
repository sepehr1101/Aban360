using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts
{
    public interface IBankAccountQueryService
    {
        Task<BankAccount> Get(short id);
        Task<ICollection<BankAccount>> Get();
    }
}
