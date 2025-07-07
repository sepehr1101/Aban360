namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PrepaymentAndCalculationHeaderOutputDto
    {
        public int CustomerNumber { get; set; }
        public string  ReadingNumber { get; set; }
        public string RequestNumber { get; set; }
        public string PostalCode { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        public string  UsageTitle { get; set; }
        public int Premises { get; set; }

        public int UnitCommeicial { get; set; }
        public int UnitDomestic { get; set; }
        public int UnitOther { get; set; }

        public int ImprovementsCommericial { get; set; }
        public int ImprovementsDomestic { get; set; }
        public int ImprovementsOverall { get; set; }

        public int  ContractualCapacity { get; set; }
        public string TotalAmount { get; set; }
       
        
        public string ZoneTitle { get; set; }
        public int PageNumber { get; set; }
        public string IssueDateJalali { get; set; }

        public int InstallmentCount { get; set; }
        public string DueDataJalali { get; set; }
        public string Payable { get; set; }
        public string PayableToPersian { get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }
        public int RecordCount { get; set; }
    }
}
