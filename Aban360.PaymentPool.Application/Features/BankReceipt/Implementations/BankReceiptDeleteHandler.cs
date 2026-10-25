using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptDeleteHandler : IBankReceiptDeleteHandler
    {
        private readonly IBankReceiptDeleteRepository _repository;
        private readonly IBankQueryService _banks;
        public BankReceiptDeleteHandler(IBankReceiptDeleteRepository repository, IBankQueryService banks)
        {
            _repository = repository;
            _banks = banks;
        }
        public async Task<BankReceiptDeleteResultDto> Handle(BankReceiptDeleteInputDto input, Guid userId, CancellationToken cancellationToken)
        {
            if (input.BankId <= 0) throw new BaseException("شناسه بانک معتبر نیست.");
            var range = BankReceiptDateRange.Get(input.UploadDate, input.UploadDate);
            cancellationToken.ThrowIfCancellationRequested();
            var bank = await _banks.Get(input.BankId);
            if (bank == null) throw new BaseException("بانک انتخاب‌شده یافت نشد.");
            return await _repository.Delete(bank.Id, range.FromUtc, range.ToUtcExclusive, userId, cancellationToken);
        }
    }
}
