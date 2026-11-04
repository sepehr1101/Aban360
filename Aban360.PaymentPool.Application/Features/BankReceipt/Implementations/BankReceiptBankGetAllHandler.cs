using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptBankGetAllHandler : IBankReceiptBankGetAllHandler
    {
        private readonly IBankReceiptBankQueryService _banks;
        public BankReceiptBankGetAllHandler(IBankReceiptBankQueryService banks) { _banks = banks; }
        public async Task<IReadOnlyCollection<BankReceiptBankDto>> Handle(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var banks = await _banks.Get();
            return banks.OrderBy(b => b.BankName).ThenBy(b => b.Id)
                .Select(b => new BankReceiptBankDto(b.Id, b.BankName, b.CentralBankCode, b._3Char)).ToArray();
        }
    }
}
