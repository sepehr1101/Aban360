namespace Aban360.SystemPool.Domain.Features.UserGuide.Entities
{
    public class Faq
    {
        public int Id { get; set; }
        public string Header { get; set; } = default!;
        public string? Icon { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreateDateTime { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime? DeleteDateTime { get; set; }
        public string DeletedBy { get; set; } = default!;
    }
}
