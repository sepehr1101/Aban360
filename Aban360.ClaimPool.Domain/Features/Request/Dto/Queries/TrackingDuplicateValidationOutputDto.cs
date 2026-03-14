namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record TrackingDuplicateValidationOutputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string NationalCode { get; set; }
        public int TrackNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string MobileNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public bool IsDuplicate { get; set; }
    }
}
