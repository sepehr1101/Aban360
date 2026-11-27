using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Dto
{
    public sealed class BankReceiptFileListItemDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public short BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public BankReceiptImportStatus Status { get; set; }
        public int? RecordCount { get; set; }
        public long? Amount { get; set; }
    }
}
