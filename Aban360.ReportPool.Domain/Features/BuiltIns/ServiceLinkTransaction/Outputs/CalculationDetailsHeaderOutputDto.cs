namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record CalculationDetailsHeaderOutputDto
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


        public int CurrentPrimises { get; set; }
        public int PreviousePrimises { get; set; }
        public int InheritedPrimises { get; set; }

        public int CurrentImprovementOverall { get; set; }
        public int PreviouseImprovementOverall { get; set; }
        public int InheritedImprovementOverall { get; set; }

        public int CurrentImprovementCommericial { get; set; }
        public int PreviouseImprovementCommericial { get; set; }
        public int InheritedImprovementCommericial { get; set; }

        public int CurrentImprovementDomestic { get; set; }
        public int PreviouseImprovementDomestic { get; set; }
        public int InheritedImprovementDomestic { get; set; }

        public int CurrentUnittOther { get; set; }
        public int PreviouseUnitOther { get; set; }
        public int InheritedUnitOther { get; set; }

        public int CurrentUnitCommericial { get; set; }
        public int PreviouseUnitCommericial { get; set; }
        public int InheritedUnitCommericial { get; set; }

        public int CurrentUnitDomestic { get; set; }
        public int PreviouseUnitDomestic { get; set; }
        public int InheritedUnitDomestic { get; set; }

        public int CurrentContractualCapacity { get; set; }
        public int PreviouseContractualCapacity { get; set; }
        public int InheritedContractualCapacity { get; set; }

        public int SumCurrentPremisesImprovement{ get; set; }
        public int InheritedPremisesImprovement{ get; set; }

        public int BaseBillId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public ICollection<SiphonDetailItemTitleDto> SiphonDetails{ get; set; }
        public string ReadingBlock { get; set; }
        public string ConfirmedType { get; set; }
        public int Storeys { get; set; }


        public string PrePayment { get; set; }
        public string EachInstallmentAmount { get; set; }
        public string Sum { get; set; }
        public string AfterDeductionSum { get; set; }
        public int PageNumber { get; set; }
        public int InstallationCount { get; set; }

    }
}
