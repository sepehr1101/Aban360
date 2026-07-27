namespace Aban360.ReportPool.Domain.Features.Tagging.Commands
{
    public record MainTagGroupRemoveDto
    {
        public int Id { get; set; }
        public DateTime RemoveDateTime { get; set; } = DateTime.Now;
        public MainTagGroupRemoveDto(int id)
        {
            Id = id;
        }
    }
}
