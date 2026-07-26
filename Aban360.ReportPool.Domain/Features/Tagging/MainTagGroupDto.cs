namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record MainTagGroupInsertInputDto
    {
        public string Title { get; set; }
    }
    public record MainTagGroupInsertDto
    {
        public string Title { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public MainTagGroupInsertDto(string title)
        {
            Title = title;
        }
    }
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
    public record MainTagGroupRemoveDto
    {
        public int Id { get; set; }
        public DateTime RemoveDateTime { get; set; } = DateTime.Now;
        public MainTagGroupRemoveDto(int id)
        {
            Id = id;
        }
    }
    public record MainTagGroupGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
