using System.Reflection.Metadata.Ecma335;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record MeterComparisonBatchDataWithCustomerInfoOutputDto
    {
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }

        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }

        public int PreviousMeterNumber { get; set; }
        public int CurrentMeterNumber { get; set; }

        public double PreviousAmount { get; set; }
        public double CurrentAmount { get; set; }
        public double PreviousDiscount { get; set; }

        public bool IsChecked{ get; set; }

        //customerInfo
        public int ZoneId { get; set; }
        public int Radif { get; set; }
        public int BranchType { get; set; }
        public int UsageId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommertialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public string WaterInstallationDateJalali { get; set; } = default!;
        public string? SewageInstallationDateJalali { get; set; }
        public string WaterRegisterDate { get; set; } = default!;
        public string? SewageRegisterDate { get; set; }
        public int WaterCount { get; set; }
        public int SewageCalcState { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string VillageId { get; set; }
        public bool IsSpecial { get; set; }
        public bool MeterDiameterId { get; set; }
        public double SumItems { get; set; }
    }
}
