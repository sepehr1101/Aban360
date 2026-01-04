namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record TopbarLevel2
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Style { get; set; }=default!;
        public int LogicalOrder { get; set; }
        public string ClientRoute { get; set; } = default!;
        public string? Description { get; set; }
    }
}
