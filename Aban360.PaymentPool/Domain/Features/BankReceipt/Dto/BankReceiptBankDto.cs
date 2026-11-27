namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Dto
{
    public sealed record BankReceiptBankDto(short Id, string BankName, string CentralBankCode, string? Identifier);
}
