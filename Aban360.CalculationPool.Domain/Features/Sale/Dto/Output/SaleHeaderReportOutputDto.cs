using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Sale.Dto.Output
{
    public record SaleHeaderReportOutputDto
    {
        public string ZoneTitle { get; set; } = "--";
        public string BillId { get; set; } = "--";
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string? RegisterDateJalali { get; set; }


        public string ReadingNumber { get; set; } = "--";
        public string FirstName { get; set; } = "--";
        public string Surname { get; set; } = "--";
        public string FatherName { get; set; } = "--";
        public string Address { get; set; } = "--";
        public string UsageTitle { get; set; } = "--";
        public string? PhoneNumber { get; set; } = "--";
        public string? MobileNumber { get; set; } = "--";
        public string RequestDateJalali { get; set; } = "--";
        public string? RequestNumber { get; set; } = "--";


        public int MotherChild { get; set; }

        public int ChildPremises { get; set; }
        public int ChildImprovementOverall { get; set; }
        public int ChildImprovementDomestic { get; set; }
        public int ChildImprovementNonDomestic { get; set; }
        public int ChildDomesticUnit { get; set; }
        public int ChildNonDomesticUnit { get; set; }
        public int ChildCommertialUnit { get; set; }
        public int ChildContractualCapacity { get; set; }
        public int ChildPremisesImprovement { get; set; }

        public int MotherPremises { get; set; }
        public int MotherImprovementOverall { get; set; }
        public int MotherImprovementDomestic { get; set; }
        public int MotherImprovementNonDomestic { get; set; }
        public int MotherDomesticUnit { get; set; }
        public int MotherNonDomesticUnit { get; set; }
        public int MotherCommertialUnit { get; set; }
        public int MotherContractualCapacity { get; set; }
        public int MotherPremisesImprovement { get; set; }

        public int PreviousPremises { get; set; }
        public int PreviousImprovementOverall { get; set; }
        public int PreviousImprovementDomestic { get; set; }
        public int PreviousImprovementNonDomestic { get; set; }
        public int PreviousDomesticUnit { get; set; }
        public int PreviousNonDomesticUnit { get; set; }
        public int PreviousCommertialUnit { get; set; }
        public int PreviousContractualCapacity { get; set; }
        public int PreviousPremisesImprovement { get; set; }


        public string MeterDiameterTitle { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }



        public bool HasBroker { get; set; }
        public long CompanyAmount { get; set; }
        public long? CompanyDiscountAmount { get; set; }
        public long CompanyFinalAmount { get; set; }
        public int CompanyItemCount { get; set; }

        public long? BrokerAmount { get; set; }
        public int? BrokerItemCount { get; set; }

        public long SumAmount { get; set; }
        public long PayableAmount { get; set; }
        public int ItemCount { get; set; }



        public long InstallmentAmount { get; set; }
        public int PerInstallmentAmount { get; set; }
        public int InstallmentCount { get; set; }

        public string? Description { get; set; }
        public string GeneralDescription { get; set; }



    }
}
