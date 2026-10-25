namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Entities
{
    public sealed class BankReceiptDiscrepancy
    {
        public Guid ImportId { get; set; }
        public int LineNumber { get; set; }
        public DateTime UploadedAt { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string BillId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string PayDate { get; set; } = string.Empty;
        public string RawData { get; set; } = string.Empty;
        public long Amount { get; set; }
        public bool IsActive { get; set; }
        public List<BankReceiptIssue> Reasons { get; set; } = new();
        public string? Description { get; set; }
        public Guid? DescribedBy { get; set; }
        public DateTime? DescribedAt { get; set; }
        public bool CanDescribe => IsActive && Description == null;
    }
}
