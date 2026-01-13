namespace Aban360.OldCalcPools.WaterReturn.Dto.Queries
{
    public record MemberGetDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public int UsageId { get; set; }
        public int BranchTypeId { get; set; }
        public int MeterDiamterId { get; set; }
        public string BodySerial { get; set; }
        public int CommertialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int EmptyUnit { get; set; }
        public int HouseholdNumber { get; set; }

    }
}
