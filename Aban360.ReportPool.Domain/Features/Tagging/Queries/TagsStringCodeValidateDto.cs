namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record TagsStringCodeValidateDto
    {
        public string StringCode { get; set; }
        public bool IsValid { get; set; }
    }
}
