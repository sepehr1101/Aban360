using System.Text.Json.Serialization;

namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record LatestWaterMeterInfoDto
    {
        public string CustomerNumber { get; set; }
        public string ZoneId { get; set; }
        public int ContractualCapacity { get; set; }
        public float ConsumptionAverage { get; set; }

        //Latest Invoice Paid
        public long WaterDebt { get; set; }
        public long BranchDebt { get; set; }
        public string LatestWaterPaid { get; set; }
        public string ConsumptionState { get; set; }

        //Latest Meter Status
        public string MeterStateTitle { get; set; }
        public string LatestMeterNumber { get; set; }
        public string MeterLife { get; set; }
        public string MeterReplacementDate { get; set; }
        public string LatestMeterReading { get; set; }

        //Latest Branch Status
        public string UseStateTitle { get; set; }
        public bool PossibilityEmptyUnit { get; set; }//todo

        [JsonIgnore]
        public int DomesticUnit { get; set; }
        [JsonIgnore]
        public int UsageId { get; set; }

        public string LatestTemporarilyDisconnectionBranch { get; set; }
        public string BranchStatus { get; set; }
        public bool CommonSiphon { get; set; }

        //Other
        public bool TagStatus { get; set; }
        public int IsContaminated { get; set; }
        public string LatestMainChangeDate { get; set; }

        public string WaterInstallationDateJalali { get; set; }
    }
}
