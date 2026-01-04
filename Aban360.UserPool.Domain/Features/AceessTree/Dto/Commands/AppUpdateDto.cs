namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands
{
    public record AppUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Style { get; set; } = null!;
        public bool InMenu { get; set; }
        public int LogicalOrder { get; set; }
    }
}
