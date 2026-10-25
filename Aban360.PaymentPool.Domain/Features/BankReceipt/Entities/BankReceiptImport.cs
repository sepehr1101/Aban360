namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Entities
{
    public enum BankReceiptImportStatus { Uploaded, Validated, Reconciled, Committed }

    public sealed class BankReceiptImport
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public short BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileHash { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; }
        public BankReceiptImportStatus Status { get; set; }
        public int Version { get; set; }
        public int DataRowCount { get; set; }
        public BankReceiptHeader? Header { get; set; }
        public List<BankReceiptRow> Rows { get; set; } = new();
        public List<BankReceiptIssue> Issues { get; set; } = new();
        public int? UploaderId { get; set; }
    }

    public sealed class BankReceiptHeader
    {
        public string UtilityCode { get; set; } = string.Empty;
        public string SubUtilityCode { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string SendDate { get; set; } = string.Empty;
        public long Amount { get; set; }
        public int RecordCount { get; set; }
    }

    public sealed class BankReceiptRow
    {
        public int LineNumber { get; set; }
        public string RawData { get; set; } = string.Empty;
        public List<BankReceiptIssue> ExclusionReasons { get; set; } = new();
        public bool IsExcluded => ExclusionReasons.Count != 0;
        public string BranchCode { get; set; } = string.Empty;
        public short ChannelType { get; set; }
        public string PayDate { get; set; } = string.Empty;
        public string BillId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string ReferenceCode { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Hash { get; set; } = string.Empty;
        public long? InvoiceId { get; set; }
        public long? InvoiceInstallmentId { get; set; }
        public List<BankReceiptIssue> Issues { get; set; } = new();
    }

    public sealed record BankReceiptIssue(string Code, string Message, int? LineNumber = null);
}
