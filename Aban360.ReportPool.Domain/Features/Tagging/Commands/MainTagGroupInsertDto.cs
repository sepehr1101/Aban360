namespace Aban360.ReportPool.Domain.Features.Tagging.Commands
{
    public record MainTagGroupInsertDto
    {
        public string Title { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public MainTagGroupInsertDto(string title)
        {
            Title = title;
        }
    }
}
