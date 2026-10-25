using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Contracts
{
    /// <summary>
    /// Implement using the authoritative subscriber, city and invoice sources.
    /// Return an entry for every input line. Report unrecognized bill IDs and cities
    /// outside Isfahan as issues; supply invoice IDs only for fully matched payments.
    /// Check payment amount, payment ID and any historical duplicates in legacy sources.
    /// </summary>
    public interface IBankReceiptReconciliationService
    {
        Task<IReadOnlyDictionary<int, BankReceiptMatch>> Reconcile(
            IReadOnlyCollection<BankReceiptRow> rows, CancellationToken cancellationToken);
    }
    public sealed record BankReceiptMatch(long? InvoiceId, long? InvoiceInstallmentId,
        IReadOnlyCollection<BankReceiptIssue> Issues);
}
