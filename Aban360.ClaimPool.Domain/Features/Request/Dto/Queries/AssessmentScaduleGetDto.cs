namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentScaduleGetDto
    {
        public Guid AssessmentId { get; set; }
        public int AssessmentCode { get; set; }
        public string AssessmentName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public bool IsVillage { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public bool IsActive { get; set; }
        public int Day0 { get; set; }
        public int Day1 { get; set; }
        public int Day2 { get; set; }
        public int Day3 { get; set; }
        public int Day4 { get; set; }
        public int Day5 { get; set; }
        public int Day6 { get; set; }
        public int Day7 { get; set; }
    }
}
