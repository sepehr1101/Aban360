namespace Aban360.ReportPool.Domain.Features.Tagging.Commands
{
    public class CreateTagGroupDto
    {
        public string Title { get; set; } = string.Empty;
        public string StringCode { get; set; }
        public int MainTagGroupId { get; set; }
    }
}
