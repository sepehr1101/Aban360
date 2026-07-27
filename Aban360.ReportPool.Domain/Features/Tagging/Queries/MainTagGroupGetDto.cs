namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record MainTagGroupGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
