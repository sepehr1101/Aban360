namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record SubscriptionAssignmentGetDto
    {
        public int Id { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName  { get; set; }
        public string SurName  { get; set; }
        public string Address { get; set; }
        public string? PostalCode { get; set; }

        public double Easting { get; set; }
        public double Northing { get; set; }
        public int UtmZone { get; set; }
        public string? Letter { get; set; }
    }
}
