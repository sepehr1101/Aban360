namespace Aban360.SystemPool.Domain.Features.UserGuide.Dtos
{
    public class FaqGetAllDto
    {
        public int Id { get; set; }
        public string Header { get; set; } = default!;
        public string? Icon { get; set; }
    }
}
