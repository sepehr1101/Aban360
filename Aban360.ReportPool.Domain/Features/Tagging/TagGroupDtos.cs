namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public class TagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreateDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }

    public class CreateTagGroupDto
    {
        public string Title { get; set; } = string.Empty;
    }

    public class UpdateTagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
