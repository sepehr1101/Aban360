namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TagGroupId { get; set; }
        public string TagGroupTitle { get; set; } = string.Empty;
        public string? StringCode { get; set; }
    }

    public class CreateTagDto
    {
        public string Title { get; set; } = string.Empty;
        public int TagGroupId { get; set; }
        public string? StringCode{ get; set; }
    }

    public class UpdateTagDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TagGroupId { get; set; }
        public string? StringCode { get; set; }

    }
}
