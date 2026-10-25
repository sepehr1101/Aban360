using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Dto
{
    public sealed class BankReceiptDisplayDto
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public BankReceiptImportStatus Status { get; set; }
        public BankReceiptHeader? Header { get; set; }
        public int ActualRecordCount { get; set; }
        public long ActualAmount { get; set; }
        public int InvalidRecordCount { get; set; }
        public int ExcludedRecordCount { get; set; }
        public bool CanConfirm { get; set; }
        public List<BankReceiptIssue> Issues { get; set; } = new();
        public List<BankReceiptRow> SampleRows { get; set; } = new();
    }
    public sealed record BankReceiptConfirmDto(int Version, bool Confirmed);
    public sealed record BankReceiptReconcileDto(int Version, int TotalCount, int InvalidCount, List<BankReceiptRow> Rows)
    {
        public int ExcludedRecordCount => Rows.Count(r => r.IsExcluded);
    }
}
