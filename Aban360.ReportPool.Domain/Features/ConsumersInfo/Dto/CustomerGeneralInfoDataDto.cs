namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CustomerGeneralInfoDataDto
    {
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
        public string? LatestPaymentDateJalali { get; set; }


        public string CounterStateCode { get; set; }
        public int LatestMeterNumber { get; set; }
        public string MeterLife { get; set; }
        public string BodySerial { get; set; }
        public string? MeterChangeDateJalali { get; set; }
        public string LatestMeterReading { get; set; }

        public string DeletionStateTitle { get; set; }
        public string UsageStatusTitle { get; set; }
        public bool CommonSiphon { get; set; }

    }
}
