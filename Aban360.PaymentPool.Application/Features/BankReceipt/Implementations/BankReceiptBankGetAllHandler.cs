using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptBankGetAllHandler : IBankReceiptBankGetAllHandler
    {
        private readonly IBankQueryService _banks;
        public BankReceiptBankGetAllHandler(IBankQueryService banks) { _banks = banks; }
        public async Task<IReadOnlyCollection<BankReceiptBankDto>> Handle(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var banks = await _banks.Get();
            return banks.OrderBy(b => b.BankName).ThenBy(b => b.Id)
                .Select(b => new BankReceiptBankDto(b.Id, b.BankName, b.CentralBankCode, b._3Char)).ToArray();
        }
    }
}
