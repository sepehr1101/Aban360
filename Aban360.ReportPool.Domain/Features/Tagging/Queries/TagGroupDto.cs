namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public class TagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StringCode { get; set; }
        public int MainTagGroupId { get; set; }
        public string MainTagGroupTitle { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }
}
