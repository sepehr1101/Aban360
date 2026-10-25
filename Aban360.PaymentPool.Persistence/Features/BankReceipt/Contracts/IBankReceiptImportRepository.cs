using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts
{
    public interface IBankReceiptImportRepository
    {
        Task Create(BankReceiptImport import, CancellationToken cancellationToken);
        Task<BankReceiptImport> Get(Guid id, Guid userId, CancellationToken cancellationToken);
        Task Save(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken);
        Task<IReadOnlySet<string>> FindCommittedPayments(IEnumerable<BankReceiptRow> rows, CancellationToken cancellationToken);
        Task<int> Commit(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken);
    }
}
