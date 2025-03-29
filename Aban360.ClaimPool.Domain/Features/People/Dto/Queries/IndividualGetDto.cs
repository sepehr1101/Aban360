namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record IndividualGetDto
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public Guid UserId { get; set; }
        public int? PreviousId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string InsertLogInfo { get; set; } = null!;
    }
}
