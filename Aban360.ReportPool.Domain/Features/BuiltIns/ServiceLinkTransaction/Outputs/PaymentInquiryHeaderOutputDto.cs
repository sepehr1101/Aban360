namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PaymentInquiryHeaderOutputDto
    {
        public string BillId { get; set; }
        public string PaymentId { get; set; }
        public string FromDateJalali { get; set; }

        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }
        public int CustomerCount { get; set; }
        public string? Title { get; set; }

    }
}
