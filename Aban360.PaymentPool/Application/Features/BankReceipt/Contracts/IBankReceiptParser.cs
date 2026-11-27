using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Contracts
{
    public interface IBankReceiptParser
    {
        void Parse(BankReceiptImport import, string bankCode, string bankIdentifier);
    }
}
