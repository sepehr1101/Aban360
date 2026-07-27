namespace Aban360.ReportPool.Domain.Features.Tagging.Commands
{
    public record MainTagGroupUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public MainTagGroupUpdateDto(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
