namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CustomerGeneralInfoDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string UsageTitle { get; set; }
        public int ContractualCapacity { get; set; }
        public string MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string MainSiphon { get; set; }
        public string BranchTypeTitle { get; set; }
        public bool DiscountType { get; set; }
        public string WaterRequestDateJalali { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string SewageRequestDateJalali { get; set; }
        public string SewageInstallationDateJalali { get; set; }


        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string PostalCode { get; set; }
        public double N { get; set; }
        public double E { get; set; }
        public string Address { get; set; }

        public int TotalUnit { get; set; }
        public int EmptyUnit { get; set; }
        public int HouseholdNumber { get; set; }


        public long WaterDebtAmount { get; set; }
        public long SewageDebtAmount { get; set; }//
        public string LatestPaymentDateJalali { get; set; }


        public string CounterStateCode { get; set; }    
        public int LatestMeterNumber { get; set; }
        public string MeterLife { get; set; }
        public string BodySerial { get; set; }
        public string? MeterChangeDateJalali { get; set; }   
        public string LatestMeterReading { get; set; }


        public string UsageStatusTitle { get; set; }
        public bool CommonSiphon { get; set; }

    }
}
