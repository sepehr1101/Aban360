namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkCalculationDetailsHeaderOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ZoneTitle { get; set; }

        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string RequestDataJalali { get; set; }
        public string UsageTitle { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }


        public int Primises { get; set; }

        public int ImprovementOverall { get; set; }
        public int ImprovementCommericial { get; set; }
        public int ImprovementDomestic { get; set; }

        public int UnitOther { get; set; }
        public int UnitCommericial { get; set; }
        public int UnitDomestic { get; set; }

        public int ContractualCapacity { get; set; }

        public int SumCurrentPremisesImprovement { get; set; }
        public int InheritedPremisesImprovement { get; set; }

        public int BaseBillId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public ICollection<SiphonDetailItemTitleDto> SiphonDetails { get; set; }
        public string ReadingBlock { get; set; }//
        public string ConfirmedType { get; set; }//
        public int Storeys { get; set; }//


        public string PrePayment { get; set; }//
        public string EachInstallmentAmount { get; set; }
        public string Sum { get; set; }
        public string AfterDeductionSum { get; set; }
        public int PageNumber { get; set; }
        public int InstallationCount { get; set; }

    }
}
