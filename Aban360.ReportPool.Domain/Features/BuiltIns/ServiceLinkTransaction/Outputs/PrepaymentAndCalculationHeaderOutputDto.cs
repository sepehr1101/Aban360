namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PrepaymentAndCalculationHeaderOutputDto
    {
        public PrepaymentAndCalculationCustomerHeaderOutputDto CustomerHeader { get; set; }
        public PrepaymentAndCalculationInstallmentHeaderOutputDto InstallmentHeader { get; set; }


        public long SumItemsAmount { get; set; }
        public long SumItemsDiscount { get; set; }
        public long DebtorOrCreditorAmount { get; set; }


        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public string PersianStringAmount { get; set; }
        public string PaymentDateJalali { get; set; }
        public string PaymentGetway { get; set; }

        public int InstallmentCount { get; set; }
        public int InstallmentNumber { get; set; }

        public string? Barcode { get; set; }

    }
    public record PrepaymentAndCalculationCustomerHeaderOutputDto
    {
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }


        public string UsageTitle { get; set; }
        public int UsageId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int BranchTypeId { get; set; }
        public int ContractualCapacity { get; set; }


        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int PageNumber { get; set; }
        public string RequestNumber { get; set; }



        public int UnitCommeicial { get; set; }
        public int UnitDomestic { get; set; }
        public int UnitOther { get; set; }


        public int Premises { get; set; }


        public int ImprovementsCommericial { get; set; }
        public int ImprovementsDomestic { get; set; }
        public int ImprovementsOverall { get; set; }

        public string ReadingBlock { get; set; }
        public string ServiceDescription { get; set; }
        public string Description { get; set; }


    }

    public record PrepaymentAndCalculationInstallmentHeaderOutputDto
    {
        public string DueDataJalali { get; set; }
        public long Payable { get; set; }
      

        public string BillId { get; set; }
        public string PaymentId { get; set; }
    }

}


