using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts
{
    public interface IBankReceiptDiscrepancyRepository
    {
        Task<BankReceiptDiscrepancyReportDto> Report(DateTime fromUtc, DateTime toUtcExclusive,
            Guid userId, int pageNumber, int pageSize, bool? hasDescription, bool? isActive, CancellationToken cancellationToken);
        Task Describe(Guid importId, int lineNumber, string description, Guid userId, CancellationToken cancellationToken);
    }
}
