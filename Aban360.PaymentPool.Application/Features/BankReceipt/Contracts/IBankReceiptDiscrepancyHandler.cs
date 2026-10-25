using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Contracts
{
    public interface IBankReceiptDiscrepancyHandler
    {
        Task<BankReceiptDiscrepancyReportDto> Report(BankReceiptDiscrepancyReportInputDto input, Guid userId, CancellationToken cancellationToken);
        Task Describe(Guid importId, int lineNumber, BankReceiptDiscrepancyDescriptionDto input, Guid userId, CancellationToken cancellationToken);
    }
}
