namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record UnconfirmedRequestDataOutputDto
    {
        public int TrackNumber { get; set; }
        public long Amount { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public int CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
    }
}
