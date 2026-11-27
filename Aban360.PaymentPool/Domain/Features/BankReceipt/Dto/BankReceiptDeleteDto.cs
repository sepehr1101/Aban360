namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Dto
{
    public sealed record BankReceiptDeleteInputDto(short BankId, string UploadDate);
    public sealed record BankReceiptDeleteResultDto(int DeletedFileCount, int CommittedFileCount);
}
