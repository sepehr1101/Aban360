namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MembersInfo
    {
        public string BillId { get; set; }
        public int UsageId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string SewageInstallationDateJalali { get; set; }
        public string WaterRegisterDate { get; set; }
        public string SewageRegisterDate { get; set; }
        public int WaterCount { get; set; }
        public int SewageCalcState { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDate { get; set; }
        public string? VillageId { get; set; }
        public bool IsSpecial { get; set; }
        public short MeterDiameterId { get; set; }
        public int VirtualCategoryId { get; set; }
    }
}
