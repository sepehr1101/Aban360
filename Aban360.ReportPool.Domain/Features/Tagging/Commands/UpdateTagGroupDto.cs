namespace Aban360.ReportPool.Domain.Features.Tagging.Commands
{
    public class UpdateTagGroupDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StringCode { get; set; }
        public int MainTagGroupId { get; set; }
    }
}
