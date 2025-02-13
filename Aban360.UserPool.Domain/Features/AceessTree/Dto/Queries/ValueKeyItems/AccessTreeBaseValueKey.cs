namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems
{
    public record AccessTreeBaseValueKey
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Style { get; set; }
        public AccessTreeBaseValueKey(int id, string title, string style)
        {
            Id=id;
            Title=title;
            Style=style;
        }
    }
}
