namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record IndividualCommandDto
    {
        //public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public Guid UserId { get; set; }
        public int? PreviousId { get; set; }
        //public DateTime ValidFrom { get; set; }
        //public DateTime? ValidTo { get; set; }
        //public string InsertLogInfo { get; set; } = null!;

        //
        public short IndividualTypeId { get; set; }

    }
}
