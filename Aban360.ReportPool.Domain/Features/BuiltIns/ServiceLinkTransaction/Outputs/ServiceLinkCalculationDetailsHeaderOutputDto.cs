namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkCalculationDetailsHeaderOutputDto
    {
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }

        public string ServiceDescription { get; set; }

        public string ReadingBlock { get; set; }
        public string MeterDiameterTitle { get; set; }
        public IEnumerable<SiphonDetailItemTitleDto> SiphonDetails { get; set; }


        public long SumNetAmount { get; set; }
        public long SumRawAmount { get; set; }
        public long Prepayment { get; set; }
        public int InstallmentCount { get; set; }
        public long InstallmentAmount { get; set; }


        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }


        public int PageNumber { get; set; }
        public string RequestNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }

        public string UsageTitle { get; set; }
        public int UsageId { get; set; }
        public string UseStateTitle { get; set; }
        public int UseStateId{ get; set; }


        public string ReportDateJalali { get; set; }


        public int CurrentPrimises { get; set; }
        public int CurrentImprovementOverall { get; set; }
        public int CurrentImprovementCommericial { get; set; }
        public int CurrentImprovementDomestic { get; set; }
        public int CurrentImprovementOther { get; set; }
        public int CurrentUnitCommericial { get; set; }
        public int CurrentUnitDomestic { get; set; }
        public int CurrentUnitOther { get; set; }
        public int CurrentContractualCapacity { get; set; }
        public int SumCurrentPremisesImprovement { get; set; }
        public ItemsHeaderOutputDto CurrentItems { get; set; }
        public ItemsHeaderOutputDto InheritedItems { get; set; }
        public ItemsHeaderOutputDto PreviousItems { get; set; }


       





        public long SumItemAmount { get; set; }

        public string Description { get; set; }


    }
}
