namespace Aban360.ClaimPool.Domain.Features._Base.Dto
{
    public record IndividualCommandBaseDto
    {
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public short IndividualTypeId { get; set; }
        public ICollection<int>? TagIds { get; set; }
    }
}
