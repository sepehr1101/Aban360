namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MoshtrakOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string NeighbourBillId { get; set; }
        public int TrackNumber { get; set; }
        public int UsageId { get; set; }
        public bool IsRegistered { get; set; }
    }
}
