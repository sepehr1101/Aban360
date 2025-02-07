namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record TopbarLevel1
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Style { get; set; } = default!;
        public int LogicalOrder { get; set; }
        public ICollection<TopbarLevel2> Level2s { get; set; } = default!;
    }
}
