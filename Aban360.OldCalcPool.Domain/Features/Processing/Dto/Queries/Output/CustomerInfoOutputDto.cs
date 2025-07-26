namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record CustomerInfoOutputDto
    {
        public int ZoneId { get; set; }
        public int Radif { get; set; }
        public int BranchType { get; set; }
        public int UsageId { get; set; }
        public int DomesticUnit { get; set; }
        public int CommertialUnit { get; set; }
        public int OtherUnit { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string SewageInstallationDateJalali { get; set; }
        public int WaterCount { get; set; }
        public int SewageCount { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdNumber { get; set; }
        public string ReadingNumber { get; set; }

    }
}
