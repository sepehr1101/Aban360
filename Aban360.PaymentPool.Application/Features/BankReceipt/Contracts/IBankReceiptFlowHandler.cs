using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Contracts
{
    public interface IBankReceiptFlowHandler
    {
        Task<Guid> Upload(Stream stream, string fileName, short bankId, Guid userId, string username, CancellationToken cancellationToken);
        Task<BankReceiptDisplayDto> Validate(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<BankReceiptDisplayDto> Display(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<BankReceiptReconcileDto> Reconcile(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<int> Confirm(Guid id, Guid userId, BankReceiptConfirmDto input, CancellationToken cancellationToken);
    }
}
