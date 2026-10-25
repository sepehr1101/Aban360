using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Contracts
{
    public interface IBankReceiptBankGetAllHandler
    {
        Task<IReadOnlyCollection<BankReceiptBankDto>> Handle(CancellationToken cancellationToken);
    }
}
