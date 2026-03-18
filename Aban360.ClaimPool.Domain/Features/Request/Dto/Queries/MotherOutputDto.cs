namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MotherOutputDto
    {
        public int TrackNumber { get; set; }
        public string StringTrackNumber {  get; set; }
        public string? BillId { get; set; }
        public int CustomerNumbe { get; set; }
        public int ZoneId { get; set; }
        public int MeterDiameterId { get; set; }
        public int UsageId { get; set; }
        public bool IsSpecial { get; set; }

        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int CommonSiphon { get; set; }

        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int Premises { get; set; }
        public int ImprovementCommercial { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementOverall { get; set; }
        public int ContractualCapacity { get; set; }
    }
}
