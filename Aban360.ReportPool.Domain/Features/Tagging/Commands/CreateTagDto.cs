namespace Aban360.ReportPool.Domain.Features.Tagging.Commands
{
    public class CreateTagDto
    {
        public string Title { get; set; } = string.Empty;
        public int TagGroupId { get; set; }
        public string StringCode { get; set; }
    }
}
