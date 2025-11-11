namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record CustomerInfoOutputDto
    {
        public int ZoneId { get; set; }
        public int Radif { get; set; }
        public string BillId { get; set; } = default!;
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
        public string? HouseholdDate { get; set; }
        public string ReadingNumber { get; set; }=default!;
        public string? VillageId { get; set; }
        public bool IsSpecial { get; set; }
        public int MeterDiameterId { get; set; }
        public int HouseholdUnit { get; set; }
        public int VirtualCategoryId { get; set; }
    }
}
