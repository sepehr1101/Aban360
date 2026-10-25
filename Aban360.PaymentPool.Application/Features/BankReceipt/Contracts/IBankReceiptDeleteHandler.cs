using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Contracts
{
    public interface IBankReceiptDeleteHandler
    {
        Task<BankReceiptDeleteResultDto> Handle(BankReceiptDeleteInputDto input, Guid userId, CancellationToken cancellationToken);
    }
}
