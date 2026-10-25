using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Domain.Features.BankReceipt.Dto
{
    // Persian dates in yyyyMMdd or yyyy/MM/dd; both endpoints of the range are inclusive.
    public sealed class BankReceiptDiscrepancyReportInputDto
    {
        public string FromDate { get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public bool? HasDescription { get; set; }
        public bool? IsActive { get; set; }
    }
    public sealed record BankReceiptDiscrepancyReportDto(int TotalCount, int PageNumber, int PageSize,
        IReadOnlyCollection<BankReceiptDiscrepancy> Items);
    public sealed record BankReceiptDiscrepancyDescriptionDto(string Description);
}
