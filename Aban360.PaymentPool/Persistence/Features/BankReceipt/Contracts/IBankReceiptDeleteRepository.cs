using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts
{
    public interface IBankReceiptDeleteRepository
    {
        Task<BankReceiptDeleteResultDto> Delete(short bankId, DateTime fromUtc, DateTime toUtcExclusive,
            Guid userId, CancellationToken cancellationToken);
    }
}
