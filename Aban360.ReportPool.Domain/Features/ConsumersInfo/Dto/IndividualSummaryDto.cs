namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record IndividualSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string? FatherName { get; set; }
        public string? NationalId { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
    }
}
