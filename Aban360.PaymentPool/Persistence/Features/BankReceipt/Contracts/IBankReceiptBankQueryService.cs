using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts
{
    public interface IBankReceiptBankQueryService
    {
        Task<Bank> Get(short id);
        Task<ICollection<Bank>> Get();
    }
}
